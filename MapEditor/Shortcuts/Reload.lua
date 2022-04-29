local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

function MyKey:onActivate(handler)
    handler.loadKeybinds()
end

function MyKey:onReciveText(text)
    
end

return MyKey
