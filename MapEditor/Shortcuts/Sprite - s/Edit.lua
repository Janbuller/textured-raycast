local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "e"

local selectedSprite = nil

local ID = -1
local extra = ""
local IDCorrespondence = {
    ["1"] = {"IDForMapToGoTo IDForDoorOfMapToGoTo MyDoorID", "Door"},
    ["2"] = {"R(0-255), G(0-255), B(0-255), Intensity(0-100), Linear, Quadratic", "Light"},
    ["3"] = {"Hp AppxDmg VarInDam xpGiven MoneyRecived MoneyVar", "Enemy"},
    ["4"] = {"FirstIndexOfList SecondIndexOfList DialougeMode", "Function sprite, mostly talking"},
    ["5"] = {"No arguments", "Choice tp"},
    ["6"] = {"Shop index", "Shop"},
    ["7"] = {"IDForItem AmountOfThatItem", "Chest"},
}

function MyKey:onActivate()
    local closest, closestI = self.handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        selectedSprite = closest
        self:startText("", "Sprite Effect ID", true)
        ID = -1
        extra = ""
    end
end

function MyKey:onReciveText(text)
    if ID == -1 then
        if text == "" then
            saveThis()
        else
            if tonumber(text) then
                ID = tonumber(text)
                self:startText("", "ExtraEffects")
            else
                self:startText(text.."-number", "Sprite Effect ID")
            end
        end
    else
        local correspndence = IDCorrespondence[tostring(ID)]
        if correspndence then
            local l1 = #string.split(correspndence[1], " ")
            local l2 = #string.split(text, " ")
            if l1 == l2 then
                extra = text
                saveThis()
            elseif l1 > l2 then
                self:startText(text.."-missing "..l1-l2, "ExtraEffects")
            elseif l2 > l1 then
                self:startText(text.."-overload "..l2-l1, "ExtraEffects")
            end
        else
            extra = text
            saveThis()
        end
    end
end

function MyKey:drawAdditionalUI()
    if self.handler.writingTo == self then
        if ID ~= -1 then
            local correspndence = IDCorrespondence[tostring(ID)]
            if correspndence then
                local f = love.graphics.getFont()
                local newVal = ignoreFirstParts(correspndence[1], getSpaces(self.handler.keybindTxt))

                local val1 = string.match(newVal, ".- ")
                if val1 then
                    local val2 = string.sub(newVal, #val1, #newVal)

                    love.graphics.setColor(self.handler.colors["BackgroundColor"])
                    love.graphics.rectangle("fill", 0, h-40, f:getWidth(val1)+4, 20)
                    love.graphics.setColor(self.handler.colors["TextColor"])
                    love.graphics.printf(val1, 2, h-38, 600, "left")
    
                    local x = f:getWidth(val1)
    
                    love.graphics.setColor(self.handler.colors["BackgroundColor"])
                    love.graphics.rectangle("fill", x, h-40, f:getWidth(val2)+4, 20)
                    love.graphics.setColor(self.handler.colors["GhostTextColor1"])
                    love.graphics.printf(val2, x+2, h-38, 600, "left")
                else
                    love.graphics.setColor(self.handler.colors["BackgroundColor"])
                    love.graphics.rectangle("fill", 0, h-40, f:getWidth(newVal)+4, 20)
                    love.graphics.setColor(self.handler.colors["TextColor"])
                    love.graphics.printf(newVal, 2, h-38, 600, "left")
                end


                love.graphics.setColor(self.handler.colors["BackgroundColor"])
                love.graphics.rectangle("fill", 0, h-60, f:getWidth(correspndence[2])+4, 20)
                love.graphics.setColor(self.handler.colors["TextColor"])
                love.graphics.printf(correspndence[2], 2, h-58, 600, "left")
            end
        else
            local correspndence = IDCorrespondence[tostring(self.handler.keybindTxt)]
            if correspndence then
                local f = love.graphics.getFont()
                love.graphics.setColor(0.2, 0.2, 0.2)
                love.graphics.rectangle("fill", 0, h-40, f:getWidth(correspndence[2])+4, 20)
                love.graphics.setColor(1, 1, 1)
                love.graphics.printf(correspndence[2], 2, h-38, 600, "left")
            end
        end
    end
end

function getSpaces(txt)
    local len = 0
    for s in string.gmatch(txt, ".-( )") do
        len = len + 1
    end
    return len
end

function ignoreFirstParts(txt, len)
    local str = ""
    local i = 0
    for s in string.gmatch(txt.." ", ".- ") do
        i = i + 1
        if len < i then
            str = str..s
        end
    end
    return string.sub(str, 1, #str-1)
end

function saveThis()
    if ID == -1 then
        ID = ""
    end

    selectedSprite[4] = ID.." "..extra
    ID = -1
end

return MyKey