local json = require "dkjson"
local s = require "ShortcutMultiChoice"

local MyKey = s:new()

MyKey.key = "w"

MyKey.dicToPass = {}

local command = ""
local updateLoop = nil

function MyKey:onActivate()
    command = ""
    updateLoop = nil
    self:genDic()
    self:overrideDic(self.dicToPass, self.handler)
end

function MyKey:onGetResult(obj)
    command = obj[3]
    self.handler.startTxt(MyKey, "", "Run for how long?", true)
end

function MyKey:onReciveText(text)
    if tonumber(text) then
        local time = tonumber(text)
        updateLoop = love.update
        print("b")
        love.update = function(dt)
            print(dt, "a")
            updateLoop(dt)
            self:runMacro(command)
            
            time = time - dt
            if time < 0 then
                love.update = updateLoop
                updateLoop = nil
            end
        end
        return
    end
    self.handler.startTxt(MyKey, text .."-be a number", "How many times to run macro?")
end

function MyKey:runMacro(command)
    for keyList in string.gmatch(command, "%((.-)%)") do
        self:tryRunKeybind(keyList)
    end
end

function MyKey:tryRunKeybind(keyCombo)
    local kurKeybind = self.handler.keybindings
    for Key in string.gmatch(keyCombo.." ", "(.-) ") do
        if kurKeybind[Key] then
            if kurKeybind[Key].keybindings then
                kurKeybind = kurKeybind[Key].keybindings
            else
                kurKeybind = kurKeybind[Key]
            end
        end
    end
    
    if kurKeybind.onActivate then
        kurKeybind:onActivate()
    end
end

function MyKey:genDic()
    local macros = self.handler.keybindings["m"].keybindings["r"]:getRelevant()
    self.dicToPass = {}
    
    for _, macro in pairs(macros) do
        table.insert(self.dicToPass, {macro.mKey, macro.mName, macro.command})
    end
end

return MyKey