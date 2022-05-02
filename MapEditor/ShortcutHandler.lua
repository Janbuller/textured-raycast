local json = require "dkjson"

local module = {}
module.w, module.h = love.graphics.getWidth(), love.graphics.getHeight()

module.keybindings = {}
module.curKeybind = nil

module.lastKeybind = nil

module.ignore = false
module.writingTo = nil
module.keybindTxt = ""
module.displayTxt = ""

module.colors = {
    ["BackgroundColor"] = {0.2, 0.2, 0.2},
    ["TextColor"] = {1, 1, 1},
    ["FolderColor"] = {0.6, 0.8, 0.9},
    ["KeybindColor"] = {0.7, 0.2, 0.7},
    ["GhostTextColor1"] = {0.6, 0.6, 0.6},
    ["GhostTextColor2"] = {0.6, 0.6, 0.6},
}

local keybindingWidth = math.floor(module.w/200)

function module.keypressed(key)
    module.passKeyToWrite(key)
    module.passKeyToKeybinds(key)
    module.passKeyToOpenKeybinds(key)
end

function module.passKeyToWrite(key)
    if module.writingTo ~= nil then
        if key == "return" or key == "escape" then
            local savedStr, saveWrite = module.keybindTxt, module.writingTo
            module.keybindTxt = ""
            module.writingTo = nil
            module.displayTxt = ""
            love.keyboard.setKeyRepeat(false)

            if key == "return" then
                saveWrite:onReciveText(savedStr, module)
            end
        elseif key == "backspace" then
            module.keybindTxt = string.sub(module.keybindTxt, 1, #module.keybindTxt-1)
        end

        return
    end
end

function module.passKeyToKeybinds(key)
    if module.curKeybind ~= nil then
        if module.curKeybind[key] ~= nil then
            if module.curKeybind[key].key ~= nil then
                local activeKeybind = module.curKeybind[key]
                module.curKeybind = nil
                activeKeybind:onActivate()
                module.lastKeybind = activeKeybind:onActivate()
                if activeKeybind.postOnActivate ~= nil then
                    activeKeybind:postOnActivate()
                end
            else
                module.curKeybind = module.curKeybind[key].keybindings
            end
        end
    end
end

function module.passKeyToOpenKeybinds(key)
    if key == "space" and love.keyboard.isDown("lshift") or key == "lshift" and love.keyboard.isDown("space") then
        if module.curKeybind == nil then
            module.curKeybind = module.keybindings
        else
            module.curKeybind = nil
        end
    end
    if key == "escape" then
        module.curKeybind = nil
    end
end

function module.savePref()
    local jsonStr = json.encode(getStringsFromKeybindings(module.keybindings))

    local f = io.open("SC.conf", "w")
    f:write(jsonStr)
    f:close()
end

function getStringsFromKeybindings(keybindings)
    local tmpStrList = {}
    for key, v in pairs(keybindings) do
        if v.saveString then
            tmpStrList[key] = v:saveString()
        elseif v.keybindings then
            tmpStrList[key] = getStringsFromKeybindings(v.keybindings)
        end
    end
    return tmpStrList
end

function module.loadPref()
    local f = io.open("SC.conf", "r")
    if f == nil then
        module.savePref()
    end
    f = io.open("SC.conf", "r")
    local str = f:read()
    sendStringsToKeybindings(json.decode(str), module.keybindings)
    f:close()
end

function sendStringsToKeybindings(list, keybindings)
    for key, v in pairs(list) do
        if type(v) == "string" then
            local success, something = pcall(keybindings[key].loadString, keybindings[key], v)
            if not success then
                print("Reloading a file\n"..something)
                keybindings[key]:loadString(keybindings[key]:saveString())
            end
        else
            sendStringsToKeybindings(list[key], keybindings[key].keybindings)
        end
    end
end

function module.textinput(t)
    if module.writingTo ~= nil then
        if module.ignore then
            module.ignore = false
            return
        end
        module.keybindTxt = module.keybindTxt..t
    end
end

function module.draw()
    module.drawShortcutUI();

    if module.writingTo ~= nil then
        module.txtDraw()
        return
    end

    if module.curKeybind == nil then
        return
    end

    local curKeysToDraw = {}

    for keyOrDir, obj in pairs(module.curKeybind) do
        table.insert(curKeysToDraw, {keyOrDir, obj.name, obj.key ~= nil})
    end

    local height = math.ceil(#curKeysToDraw/keybindingWidth)

    love.graphics.setColor(module.colors["BackgroundColor"])
    love.graphics.rectangle("fill", 0, module.h- height*20, module.w, height*20)

    local endRes = {}
    local keys = {}

    for _, key in pairs(curKeysToDraw) do
        if key[3] == false then
            endRes[key[1]] = key
        else
            keys[key[1]] = key
        end
    end
    
    local i = 1
    i = drawPairs(endRes, i, height)
    drawPairs(keys, i, height)
end

function drawPairs(toUse, i, height)
    for _, key in pairsByKeys(toUse) do
        local xOffset = (math.ceil(i/height) - 1)
        local yOffset = (module.h-height*20) + ((i-1)*20 - xOffset*(height*20))+4
        xOffset = ((xOffset)*(module.w/keybindingWidth)+4)

        love.graphics.setColor(module.colors["TextColor"])
        love.graphics.printf(key[1], xOffset, yOffset, module.w, "left")
        love.graphics.printf("-", xOffset+20, yOffset, module.w, "left")
        
        love.graphics.setColor(module.colors["FolderColor"])
        if key[3] == true then
            love.graphics.setColor(module.colors["KeybindColor"])
        end
        love.graphics.printf(cutDown(key[2], 20), xOffset+40, yOffset, module.w, "left")

        i = i + 1
    end
    return i
end

function pairsByKeys (t, f)
    local a = {}
    for n in pairs(t) do table.insert(a, n) end
    table.sort(a, f)
    local i = 0      -- iterator variable
    local iter = function ()   -- iterator function
        i = i + 1
        if a[i] == nil then return nil
        else return a[i], t[a[i]]
        end
    end
    return iter
end

function module.drawShortcutUI()
    drawUI(module.keybindings)
end

function drawUI(keybindings)
    for _, v in pairs(keybindings) do
        if v.drawAdditionalUI then
            v:drawAdditionalUI()
        elseif v.keybindings then
            drawUI(v.keybindings)
        end
    end
end

function module.txtDraw()
    love.graphics.setColor(module.colors["BackgroundColor"])
    love.graphics.rectangle("fill", 0, module.h- 20, module.w, 20)

    if module.keybindTxt == "" then
        love.graphics.setColor(module.colors["GhostTextColor1"])
        love.graphics.printf(module.displayTxt, 2, module.h-18, module.w, "left")
    end

    love.graphics.setColor(module.colors["TextColor"])
    love.graphics.printf(module.keybindTxt, 2, module.h-18, module.w, "left")
end

function module.startTxt(keybind, preTxt, dispTxt, ignore)
    module.keybindTxt = preTxt or ""
    module.displayTxt = dispTxt or ""
    module.writingTo = keybind
    module.ignore = ignore or false
    love.keyboard.setKeyRepeat(true)
end

function cutDown(string, len)
    if #string > len then
        return string.sub(string, 0, len-3).."..."
    else
        return string
    end
end

function module.load()
    module.loadKeybinds()
    SetupKeys(module.keybindings)
end

function SetupKeys(keybindings)
    for _, v in pairs(keybindings) do
        if v.setup then
            v:setup()
        elseif v.keybindings then
            SetupKeys(v.keybindings)
        end
    end
end

function module.loadKeybinds()
    local path = love.filesystem.getWorkingDirectory().."/Shortcuts"
    module.keybindings = module.getKeysFromDir(path)

    module.loadPref()
end

function module.getKeysFromDir(path)
    local files = module.scandir(path)
    
    local keybindings = {}

    for _, file in pairs(files) do
        if (string.sub(file, #file-3, #file) == ".lua") then
            local func = loadfile(path.."/"..file)
            assert(func, "\nsomething wrong with "..path.."/"..file.."\naka "..file)
            local keybind = func()
            assert(keybind, "\nsomething wrong with "..path.."/"..file.."\naka "..file)
            if keybind.name == "" then
                keybind.name = string.sub(file, 1, #file-4)
            end
            keybind.handler = module
            keybindings[keybind.key] = keybind
        else
            local key = string.sub(file, #file, #file)
            local name = string.sub(file, 0, #file-4)
            keybindings[key] = {name = name, keybindings = module.getKeysFromDir(path.."/"..file)}
        end
    end

    return keybindings
end

function module.scandir(directory)
    local i, t, popen = 0, {}, io.popen
    local pfile;
    local platform = love.system.getOS()
    
    if platform == "Linux" or platform == "OS X" then
        pfile = popen('ls "'..directory..'"')
    elseif platform == "Windows" then
        pfile = popen('dir "'..directory..'" /b')
    end

    for filename in pfile:lines() do
        i = i + 1
        t[i] = filename
    end
    pfile:close()
    return t
end

function module.script_path()
    local str = debug.getinfo(2, "S").source:sub(2)
    return str:match("(.*/)")
end

return module