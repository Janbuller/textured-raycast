local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "e"

local selectedSprite = nil

local ID = -1
local extra = ""
local IDCorrespondence = {
    ["1"] = "IDForMapToGoTo IDForDoorOfMapToGoTo MyDoorID"
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
            local l1 = #string.split(correspndence, " ")
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
    if ID ~= -1 then
        local correspndence = IDCorrespondence[tostring(ID)]
        if correspndence then
            local f = love.graphics.getFont()
            love.graphics.setColor(0.2, 0.2, 0.2)
            love.graphics.rectangle("fill", 0, h-40, f:getWidth(correspndence)+4, 20)
            love.graphics.setColor(1, 1, 1)
            love.graphics.printf(correspndence, 2, h-38, 600, "left")
        end
    end
end

function saveThis()
    if ID == -1 then
        ID = ""
    end

    selectedSprite[4] = ID.." "..extra
    ID = -1
end

return MyKey