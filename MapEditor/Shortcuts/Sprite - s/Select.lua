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

function MyKey:drawAdditionalUI()
    if selectedSprite then
        local mX, mY = love.mouse.getPosition()
        if mx == -1 then
            mX, mY = mx, my
        end

        love.graphics.setLineWidth(0.02)
        love.graphics.translate(w/2, h/2)
        love.graphics.scale(scale, scale)
        love.graphics.translate(gridOffsetX+(mX-mx)/scale, gridOffsetY+(mY-my)/scale)

        local roundDown = math.floor(globalSpriteIndexHelper)
        local maxSprites = #selectedSprite[3]
        local thisIndex = roundDown%maxSprites+1
        
        local thisImg = folders[selectedSprite[3][thisIndex][1]][selectedSprite[3][thisIndex][2]]

        love.graphics.setColor(1, 1, 0)
        love.graphics.draw(thisImg[1], selectedSprite[1]-0.3, selectedSprite[2]-0.3, 0, (0.6/thisImg[1]:getWidth()), (0.6/thisImg[1]:getHeight()))
        
        love.graphics.origin()
        love.graphics.setLineWidth(2)
    end
end

return MyKey