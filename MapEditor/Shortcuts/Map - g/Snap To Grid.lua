local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "g"

function MyKey:onActivate()
    gridActive = not gridActive
end

return MyKey
