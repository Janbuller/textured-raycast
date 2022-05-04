local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

function MyKey:onActivate()
    local px, py = getMouseWorldPos()
    local v = math.floor(math.atan2(py-spawn[2], px-spawn[1])/(math.pi/2)+math.pi/8)*(math.pi/2)
    spawnLook = {math.cos(v), math.sin(v)}
end

return MyKey