local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "n"

local name = ""
local key = ""
local command = ""

function MyKey:onActivate()
    name = ""
    key = ""
    command = ""
    self.handler.startTxt(MyKey, "", "What to call the new macro?", true)
end

function MyKey:onReciveText(text)
    if name == "" then
        name = text
        self.handler.startTxt(MyKey, "", "What key to use?")
        return
    elseif key == "" then
        key = text
        self.handler.startTxt(MyKey, "", "Give it a command to run")
        return
    elseif command == "" then
        command = text
        local macros = self.handler.keybindings["m"].keybindings["r"]:getRelevant()

        if macros[key] then
            self.handler.startTxt(MyKey, "", "An existing macro wil be overridden, sure you will continues Y/n")
            return
        end
    elseif text == "n" then
        return
    end

    local macros = self.handler.keybindings["m"].keybindings["r"]:getRelevant()
    macros[key] = {mKey = key, mName = name, command = command}
end

return MyKey