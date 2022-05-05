local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

function MyKey:onActivate()
    local saveEffect = self.handler.keybindings["s"].keybindings["e"].keybindings["c"]:getRelevant()

    local px, py = getMouseWorldPos()

    local closest = nil
    local distance = 1
    for i,sprite in ipairs(sprites) do
        local thisDist = math.dist(sprite[1], sprite[2], px, py)
        if thisDist < distance then
            distance = thisDist
            closest = sprite
        end
    end

    if closest then
        closest[4] = saveEffect
    end
end

return MyKey