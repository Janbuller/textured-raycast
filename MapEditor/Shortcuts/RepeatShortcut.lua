local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "a"

local savedLast = nil

function MyKey:onActivate()
    if self.handler.lastKeybind then
        if MyKey ~= self.handler.lastKeybind then
            savedLast = self.handler.lastKeybind
            self.handler.lastKeybind:onActivate(self.handler)
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