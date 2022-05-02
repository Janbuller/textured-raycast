local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "a"

local savedFunc = {}

function MyKey:onActivate()
    self:startText("", "What to call the new theme?", true)
end

function MyKey:onReciveText(text)
    local colors, selected = self.handler.keybindings["c"].keybindings["s"]:getRelevant()

    if colors[text] then
        self:startText(text .. " is already taken", "What to call the new theme?")
        return
    end

    colors[text] = {
        ["BackgroundColor"] = {0.2, 0.2, 0.2},
        ["TextColor"] = {1, 1, 1},
        ["FolderColor"] = {0.6, 0.8, 0.9},
        ["KeybindColor"] = {0.7, 0.2, 0.7},
        ["GhostTextColor1"] = {0.6, 0.6, 0.6},
        ["GhostTextColor2"] = {0.6, 0.6, 0.6},
    }
end

return MyKey