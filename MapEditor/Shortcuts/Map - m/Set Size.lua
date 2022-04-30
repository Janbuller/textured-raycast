local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "s"

function MyKey:onActivate(handler)
    handler.startTxt(MyKey, "", "In format 'x y'", true)
end

function MyKey:onReciveText(text, handler)
    local nrs = string.numsplit(text, " ")
    if #nrs == 2 then
        if nrs[1] % 2 == 1 and nrs[2] % 2 == 1 then
            if nrs[1] and nrs[2] then
                grid = newGrid(nrs[1], nrs[2])
                setSize = nrs[1] .. " | " .. nrs[2]
                return
            end
        else
            handler.startTxt(MyKey, text .."-only even", "In format 'x y'")
        end
    end
    handler.startTxt(MyKey, text, "In format 'x y'")
end

return MyKey
