local json = require "dkjson"
local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

local doAutoSave = false
local timer = 0

function MyKey:onActivate()
    doAutoSave = not doAutoSave
    local name = "active"
    if not isToggled then name = "dissabled" end
    self.name = "AutoSavePrefs - "..name
end

function MyKey:setup()
    local func = love.update
    love.update = function (dt)
        func(dt)
        
        if doAutoSave then
            timer = timer + dt
            if timer > 1 then
                self.handler.savePref()
            end
        end
    end
end

function MyKey:tryRunKeybind(keyCombo)
    local kurKeybind = self.handler.keybindings
    for Key in string.gmatch(keyCombo.." ", "(.-) ") do
        if kurKeybind[Key] then
            if kurKeybind[Key].keybindings then
                kurKeybind = kurKeybind[Key].keybindings
            else
                if kurKeybind[Key].onActivate then
                    kurKeybind[Key]:onActivate()
                    kurKeybind = self.handler.keybindings
                end
            end
        end
    end
end

function MyKey:loadString(v)
    doAutoSave = json.decode(v)
    local name = "active"
    if not doAutoSave then name = "dissabled" end
    self.name = "AutoSavePrefs - "..name
end

function MyKey:saveString()
    return json.encode(doAutoSave)
end

return MyKey