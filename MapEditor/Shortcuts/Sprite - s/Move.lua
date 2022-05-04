local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "m"

local savedFunc = {}

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        savedFunc = {love.keypressed, love.mousemoved}
    
        function love.keypressed(key)
            if key == "escape" then
                love.keypressed = savedFunc[1]
                love.mousemoved = savedFunc[2]
            end
        end
    
        function love.mousemoved(x, y, dx, dy)
            local npx, npy = getMouseWorldPos()
            if gridActive then
                closest[1], closest[2] = math.ceil((npx-0.25)*2)/2, math.ceil((npy-0.25)*2)/2
            else
                closest[1], closest[2] = npx, npy
            end
        end
    end
end

return MyKey