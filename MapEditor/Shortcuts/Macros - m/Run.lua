local json = require "dkjson"
local s = require "ShortcutMultiChoice"

local MyKey = s:new()

MyKey.key = "r"

MyKey.dicToPass = {}

local macros = {} -- {mKey = key, mName = name, command = shortcutlist}
-- shortcutlist = a string, each segment: () | fx [g l] [h s]

function MyKey:onActivate()
    self:genDic()
    self:overrideDic(self.dicToPass, self.handler)
end

function MyKey:onGetResult(obj)
    self:runMacro(obj[2])
end

function MyKey:runMacro(command)
    for keyList in string.gmatch(command, "%((.-)%)") do
        print(keyList)
    end
end

function MyKey:tryRunKeybind(keyCombo)
    local kurKeybind = self.handler.keybindings
    for Key in string.gmatch(keyCombo.." ", "(.-) ") do
        if self.handler.keybindings[Key] then
            if self.handler.keybindings[Key].keybindings then
                kurKeybind = self.handler.keybindings[Key]
            end
        end
    end

    if kurKeybind.onActivate then
        kurKeybind:onActivate()
    end
end

function MyKey:genDic()
    self.dicToPass = {}
    
    for _, macro in pairs(macros) do
        table.insert(self.dicToPass, {macro.mKey, macro.mName, macro.command})
    end
end

function MyKey:getRelevant()
    return macros
end

function MyKey:loadString(v)
    self.handler.colors = json.decode(v)
end

function MyKey:saveString()
    return json.encode(macros)
end

MyKey:runMacro("(dasd da dsad) (dwqd dqwd qwd)")

return MyKey