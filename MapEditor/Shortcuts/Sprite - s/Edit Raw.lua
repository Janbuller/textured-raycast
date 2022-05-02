local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "r"

local selectedSprite = nil

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        selectedSprite = closest
        self:startText(selectedSprite[4], "", true)
    end
end

function MyKey:onReciveText(text)
    selectedSprite[4] = text
end

return MyKey