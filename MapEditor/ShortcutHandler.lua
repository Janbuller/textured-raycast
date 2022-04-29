local module = {}

local w, h = love.graphics.getWidth(), love.graphics.getHeight()
module.keybindings = {}
module.curKeybind = nil

module.ignore = false
module.writingTo = nil
module.keybindTxt = ""
module.displayTxt = ""

local keybindingWidth = 8

function module.keypressed(key)
    if module.writingTo ~= nil then
        if key == "return" then
            module.writingTo:onReciveText(module.keybindTxt)
            module.keybindTxt = ""
            module.writingTo = nil
            module.displayTxt = ""
            love.keyboard.setKeyRepeat(false)
        elseif key == "backspace" then
            module.keybindTxt = string.sub(module.keybindTxt, 1, #module.keybindTxt-1)
        end

        return
    end

    if module.curKeybind ~= nil then
        if module.curKeybind[key] ~= nil then
            if module.curKeybind[key].key ~= nil then
                module.curKeybind[key]:onActivate(module)
                module.curKeybind = nil
            else
                module.curKeybind = module.curKeybind[key].keybindings
            end
        end
    end

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

function module.txtIn(t)
    if module.writingTo ~= nil then
        if module.ignore then
            module.ignore = false
            return
        end
        module.keybindTxt = module.keybindTxt..t
    end
end

function module.draw()
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

    love.graphics.setColor(0.2, 0.2, 0.2)
    love.graphics.rectangle("fill", 0, h- height*20, w, height*20)

    local endRes = {}
    local keys = {}

    for _, key in pairs(curKeysToDraw) do
        if key[3] == false then
            table.insert(endRes, key)
        else
            table.insert(keys, key)
        end
    end

    for _, key in pairs(keys) do
        table.insert(endRes, key)
    end

    
    for i, key in ipairs(endRes) do
        local xOffset = (math.ceil(i/height) - 1)
        local yOffset = (h-height*20) + ((i-1)*20 - xOffset*(height*20))+4

        xOffset = ((xOffset)*(w/keybindingWidth)+4)

        love.graphics.setColor(1, 1, 1)
        love.graphics.printf(key[1], xOffset, yOffset, w, "left")
        love.graphics.printf("-", xOffset+20, yOffset, w, "left")
        
        love.graphics.setColor(0.2, 0.2, 1)
        if key[3] == true then
            love.graphics.setColor(0.7, 0.2, 0.7)
        end
        love.graphics.printf(cutDown(key[2], 20), xOffset+40, yOffset, w, "left")
    end
end

function module.txtDraw()
    love.graphics.setColor(0.2, 0.2, 0.2)
    love.graphics.rectangle("fill", 0, h- 20, w, 20)

    if module.keybindTxt == "" then
        love.graphics.setColor(0.6, 0.6, 0.6)
        love.graphics.printf(module.displayTxt, 2, h-18, w, "left")
    end
    love.graphics.setColor(1, 1, 1)
    love.graphics.printf(module.keybindTxt, 2, h-18, w, "left")
end

function module.startTxt(keybind, dispTxt)
    module.displayTxt = dispTxt or ""
    module.writingTo = keybind
    module.ignore = true
    love.keyboard.setKeyRepeat(true)
end

function cutDown(string, len)
    return string.sub(string, 0, len-3).."..."
end

function module.loadKeybinds()
    local path = love.filesystem.getWorkingDirectory().."/Shortcuts"
    module.keybindings = module.getKeysFromDir(path)
end

function module.getKeysFromDir(path)
    local files = module.scandir(path)
    
    local keybindings = {}

    for _, file in pairs(files) do
        if (string.sub(file, #file-3, #file) == ".lua") then
            local func = loadfile(path.."/"..file)
            local keybind = func()
            if keybind.name == "" then
                keybind.name = string.sub(file, 1, #file-4)
            end
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
