local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "d"

local selectedSprite = nil

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closestI ~= 0 then
        table.remove(sprites, closestI)
    end
end

return MyKey