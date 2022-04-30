local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "d"

local selectedSprite = nil

function MyKey:onActivate(handler)
    local closest, closestI = handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closestI ~= 0 then
        table.remove(sprites, closestI)
    end
end

return MyKey