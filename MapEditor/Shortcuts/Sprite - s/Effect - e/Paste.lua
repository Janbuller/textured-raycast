local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

function MyKey:onActivate(handler)
    local saveEffect = handler.keybindings["s"].keybindings["e"].keybindings["c"]:getRelevant()

    local mx, my = love.mouse.getPosition()
    local px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)

    local closest = nil
    local distance = 1
    for i,sprite in ipairs(sprites) do
        local thisDist = math.dist(sprite[1], sprite[2], px, py)
        print(thisDist, distance, thisDist < distance)
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