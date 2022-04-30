local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "l"

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    closest[1] = closest[1] + 1
end

return MyKey