local s = require "Shortcut"
local MultiChoice = s:new()

MultiChoice.dicToPass = {}

function MultiChoice:onActivate(handler)
    self:overrideDic(self.dicToPass, handler)
end

function MultiChoice:onGetResult(obj)

end

function MultiChoice:overrideDic(list, handler)
    handler.curKeybind = {}

    for _, v in pairs(list) do
        handler.curKeybind[v[1]] = {["onActivate"] = function()
            self:onGetResult(v)
        end, key = v[1], name = v[2]}
    end
end


return MultiChoice