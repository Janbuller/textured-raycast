local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

local selectedSprite = nil

function MyKey:onActivate(handler)
    local closest, closestI = handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        selectedSprite = closest
        handler.startTxt(MyKey, selectedSprite[4], "", true)
    end
end

function MyKey:onReciveText(text, handler)
    selectedSprite[4] = text
end

return MyKey