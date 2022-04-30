local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "o"

function MyKey:onActivate(handler)
    local closest, closestI = handler.keybindings["s"].keybindings["v"]:getRelevant()

    if closest then
        if selected[1] ~= "" then
            closest[3] = {}
            for i, selected in ipairs(multiSelect) do
                closest[3][i] = selected
            end
        end
    end
end

return MyKey