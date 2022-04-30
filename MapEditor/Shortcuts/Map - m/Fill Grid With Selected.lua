local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "f"

function MyKey:onActivate(handler)
    for x = 1,gW do
        for y = 1,gH do
            grid[gridLayer][x][y][2] = selected
        end
    end
end

return MyKey
