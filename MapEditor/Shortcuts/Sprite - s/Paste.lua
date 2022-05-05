local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

function MyKey:onActivate()
    local cloneSave = self.handler.keybindings["s"].keybindings["c"]:getRelevant()

    local px, py = getMouseWorldPos()

    if cloneSave[1] then
        if gridActive then
            table.insert(sprites, {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2, cloneSave[1], cloneSave[2]})
        else
            table.insert(sprites, {px, py, cloneSave[1], cloneSave[2]})
        end
    end
end

return MyKey