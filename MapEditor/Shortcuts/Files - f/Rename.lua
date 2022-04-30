local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

local name = ""

function MyKey:onActivate()
    self.handler.startTxt(MyKey, "", "Set new name", true)
end

function MyKey:onReciveText(text)
   self:parseName(text, self.handler)
end

function MyKey:parseName(text, handler)
    if #string.split(text, " ") == 1 then
        name = text
	fileName = name
        return
    end

    handler.startTxt(MyKey, text.." -no spaces", "Set new name")
end

return MyKey
