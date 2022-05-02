local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "a"

local savedFunc = {}

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        savedFunc = {love.keypressed}
    
        function love.keypressed(key)
            if key == "escape" then
                love.keypressed = savedFunc[1]
            elseif key == "h" then
                closest[1] = closest[1] - 1
            elseif key == "j" then
                closest[2] = closest[2] + 1
            elseif key == "k" then
                closest[2] = closest[2] - 1
            elseif key == "l" then
                closest[1] = closest[1] + 1
            end
        end
    end
end

return MyKey