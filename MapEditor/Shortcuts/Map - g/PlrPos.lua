local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

function MyKey:onActivate()
    local px, py = getMouseWorldPos()
    if gridActive then
        spawn = {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2}
    else
        spawn = {px, py}
    end
end

return MyKey