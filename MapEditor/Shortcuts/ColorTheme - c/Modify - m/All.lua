local s = require "ShortcutMultiChoice"

local MyKey = s:new()

MyKey.key = "a"

MyKey.dicToPass = {}

local palletToModify = ""
local colorToModify = ""

function MyKey:onActivate()
    palletToModify = ""
    colorToModify = ""

    self:genDic1()
    self:overrideDic(self.dicToPass, self.handler)
end

function MyKey:toString(color)
    return color[1].." "..color[2].." "..color[3]
end

function MyKey:onGetResult(obj)
    if palletToModify == "" then
        palletToModify = obj[2]
    
        self:genDic2()
        self:overrideDic(self.dicToPass, self.handler)
    elseif colorToModify == "" then
        local colors, selected = self.handler.keybindings["c"].keybindings["s"]:getRelevant()
        colorToModify = obj[2]

        self:startText(self:toString(colors[palletToModify][colorToModify]), "What color should it be?", true)
    end
end

function MyKey:onReciveText(text)
    local colorsToModify, selected = self.handler.keybindings["c"].keybindings["s"]:getRelevant()
    local colors = self:getColors(text)

    if #colors == 3 then
        if colors[1] and colors[2] and colors[3] then
            colorsToModify[palletToModify][colorToModify] = {colors[1], colors[2], colors[3]}
            return
        end
    end

    self:startText(text, "What color should it be?")
end

function MyKey:genDic1()
    local colors, selected = self.handler.keybindings["c"].keybindings["s"]:getRelevant()
    self.dicToPass = {}
    
    local i = 1
    for key, color in pairs(colors) do
        table.insert(self.dicToPass, {tostring(i), key})
        i = i + 1
    end
end

function MyKey:genDic2()
    local colors, selected = self.handler.keybindings["c"].keybindings["s"]:getRelevant()
    self.dicToPass = {}

    for key, color in pairs(colors[palletToModify]) do
        table.insert(self.dicToPass, {string.lower(string.sub(key, 1, 1)), key})
    end
end

function MyKey:getColors(text)
    local color = {}
    for res in string.gmatch(text.." ", "(.-) ") do
        if tonumber(res) then
            table.insert(color, tonumber(res))
        else
            return false
        end
    end

    if #color == 3 then
        return color
    else
        return false
    end
end

return MyKey