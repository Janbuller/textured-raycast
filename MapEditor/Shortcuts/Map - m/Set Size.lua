local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "s"

function MyKey:onActivate(handler)
end

function MyKey:onReciveText(text)
end

return MyKey
