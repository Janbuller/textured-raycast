local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "c"

local cloneSave = nil

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        cloneSave = {}
        cloneSave[1] = closest[3]
        cloneSave[2] = closest[4]
    end
end

function MyKey:getRelevant()
    return cloneSave
end

return MyKey