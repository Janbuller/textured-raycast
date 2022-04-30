local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "c"

local saveEffect = nil

function MyKey:onActivate(handler)
    local closest, closestI = handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        saveEffect = closest[4]
    end
end

function MyKey:getRelevant()
    return saveEffect
end

return MyKey