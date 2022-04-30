local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "f"

function MyKey:onActivate(handler)
    local x, y = love.mouse.getPosition()

    local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
    if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
        local selectedTile = grid[gridLayer][pointX][pointY][2]
        local toOverrideWith = selected

        if not(checkPath(selectedTile, toOverrideWith)) then
            doItAll(pointX, pointY, toOverrideWith, selectedTile, 0, 0)
        end
    end
end

function doItAll(x, y, path, pathToOverride, plusX, plusY)
    local DoSpread = x+plusX >= 1 and x+plusX <= gW and y+plusY >= 1 and y+plusY <= gH
    if DoSpread then if overrideTile(x+plusX, y+plusY, path, pathToOverride) then spreadToAround(x+plusX, y+plusY, path, pathToOverride) end end
end

function spreadToAround(x, y, path, pathToOverride)
    doItAll(x, y, path, pathToOverride, 1, 0)
    doItAll(x, y, path, pathToOverride, -1, 0)
    doItAll(x, y, path, pathToOverride, 0, 1)
    doItAll(x, y, path, pathToOverride, 0, -1)
end

function overrideTile(x, y, path, pathToOverride)
    if checkPath(grid[gridLayer][x][y][2], pathToOverride) then
        grid[gridLayer][x][y][2] = path
        return true
    end
end

function checkPath(p1, p2)
    if p1[1] == p2[1] and p1[2] == p2[2] then
        return true
    end
end

return MyKey