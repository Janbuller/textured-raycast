local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "j"

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    closest[2] = closest[2] + 1
end

return MyKey