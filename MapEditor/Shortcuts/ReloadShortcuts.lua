local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

function MyKey:onActivate(handler)
    handler.savePref()
    handler.loadKeybinds()
end

return MyKey