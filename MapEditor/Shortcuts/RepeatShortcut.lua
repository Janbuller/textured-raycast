local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "a"

local savedLast = nil

function MyKey:onActivate(handler)
    if handler.lastKeybind then
        if MyKey ~= handler.lastKeybind then
            savedLast = handler.lastKeybind
            handler.lastKeybind:onActivate(handler)
        end
    end
end

function MyKey:postOnActivate(handler)
    if savedLast then
        handler.lastKeybind = savedLast
        savedLast = nil
    end
end

return MyKey