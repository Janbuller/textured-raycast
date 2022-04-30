local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

local spawnPlacing = 1

function MyKey:onActivate(handler)
    local mx, my = love.mouse.getPosition()
    local px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)
    if spawnPlacing == 1 then
        if gridActive then
            spawn = {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2}
        else
            spawn = {px, py}
        end
        spawnPlacing = 2
    else
        local v = math.floor(math.atan2(py-spawn[2], px-spawn[1])/(math.pi/2)+math.pi/8)*(math.pi/2)
        spawnLook = {math.cos(v), math.sin(v)}
        spawnPlacing = 1
    end
end

return MyKey