local Shortcut = {name = "", key = ""}

function Shortcut:new()
    local Shortcut = {}
    setmetatable(Shortcut, self)
    self.__index = self
    return Shortcut
end

function onActivate(handeler)
    
end

function onReciveText(text)
    
end

return Shortcut