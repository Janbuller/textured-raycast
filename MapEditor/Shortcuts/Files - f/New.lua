local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "n"

local defining = 1
local mySize = {0, 0}
local name = ""

function MyKey:onActivate()
    defining = 1
    self:startText("", "Set size in format 'x y'", true)
end

function MyKey:onReciveText(text)
    if defining == 1 then
        self:parseSize(text)
    elseif defining == 2 then
        self:parseName(text)
    end
end

function MyKey:parseSize(text)
    local nrs = string.numsplit(text, " ")
    if #nrs == 2 then
        if nrs[1] and nrs[2] then
            if nrs[1] % 2 == 0 and nrs[2] % 2 == 0 then
                mySize = {nrs[1], nrs[2]}
                defining = defining + 1
                self:startText("", "What to call the map?")
                return
            else
                self:startText(text .."-only evens", "In format 'x y'")
                return
            end
        end
    end

    self:startText(text, "Set name")
end

function MyKey:parseName(text)
    if #string.split(text, " ") == 1 then
        name = text
        defining = defining + 1
        self:makeFile()
        return
    end

    self:startText(text.." -no spaces", "Set name")
end

function MyKey:makeFile()
    sprites = {}
    grid = newGrid(mySize[1], mySize[2])
    fileName = name
end

return MyKey
