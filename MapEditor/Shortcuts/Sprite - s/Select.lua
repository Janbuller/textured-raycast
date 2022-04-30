local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "v"

local selectedSprite = nil
local selectedSpriteIndex = 0

function MyKey:onActivate(handeler)
    local mx, my = love.mouse.getPosition()
    local px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)

    local closest = nil
    local closestIndex = 0
    local distance = 1
    for i,sprite in ipairs(sprites) do
        local thisDist = math.dist(sprite[1], sprite[2], px, py)
        if thisDist < distance then
            distance = thisDist
            closest = sprite
            closestIndex = i
        end
    end

    if closest then
        selectedSprite = closest
        selectedSpriteIndex = closestIndex
    end
end

function MyKey:getRelevant()
    return selectedSprite, selectedSpriteIndex
end

return MyKey