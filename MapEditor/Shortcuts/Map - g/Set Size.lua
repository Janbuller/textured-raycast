local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "s"

function MyKey:onActivate()
    self:startText("", "In format 'x y'", true)
end

function MyKey:onReciveText(text)
    local nrs = string.numsplit(text, " ")
    if #nrs == 2 then
        if nrs[1] and nrs[2] then
            if nrs[1] % 2 == 0 and nrs[2] % 2 == 0 then
                grid = newGrid(nrs[1], nrs[2])
                setSize = nrs[1] .. " | " .. nrs[2]
                return
            else
                self:startText(text .."-only even", "In format 'x y'")
            end
        end
    end
    self:startText(text, "In format 'x y'")
end

return MyKey
