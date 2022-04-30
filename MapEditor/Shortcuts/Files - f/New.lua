local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "n"

local defining = 1
local mySize = {0, 0}
local name = ""

function MyKey:onActivate(handler)
    defining = 1
    handler.startTxt(MyKey, "", "Set size in format 'x y'", true)
end

function MyKey:onReciveText(text, handler)
    if defining == 1 then
        self:parseSize(text, handler)
    elseif defining == 2 then
        self:parseName(text, handler)
    end
end

function MyKey:parseSize(text, handler)
    local nrs = string.numsplit(text, " ")
    if #nrs == 2 then
        if nrs[1] and nrs[2] then
            if nrs[1] % 2 == 0 and nrs[2] % 2 == 0 then
                mySize = {nrs[1], nrs[2]}
                defining = defining + 1
                handler.startTxt(MyKey, "", "What to call the map?")
                return
            else
                handler.startTxt(MyKey, text .."-only evens", "In format 'x y'")
                return
            end
        end
    end

    handler.startTxt(MyKey, text, "Set size in format 'x y'")
end

function MyKey:parseName(text, handler)
    if #string.split(text, " ") == 1 then
        name = text
        defining = defining + 1
        self:makeFile()
        return
    end

    handler.startTxt(MyKey, text " -no spaces", "Set size in format 'x y'")
end

function MyKey:makeFile()
    sprites = {}
    grid = newGrid(mySize[1], mySize[2])
    fileName = name
end

return MyKey
