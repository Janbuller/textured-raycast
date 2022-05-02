local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "o"

local savedKeyInput = nil
local input = ""

function MyKey:onActivate()
    if savedKeyInput == nil then
        self.name = "Disable MultiInput"
        savedKeyInput = self.handler.passKeyToKeybinds

        self.handler.passKeyToKeybinds = function (key)
            if self.handler.curKeybind ~= nil then
                if key == "return" then
                    savedKeyInput(input)
                    input = ""
                elseif key == "escape" then
                    if input == "" then
                        self.handler.passKeyToKeybinds = savedKeyInput
                        savedKeyInput = nil
                    else
                        input = ""
                    end
                else
                    input = input..key
                end
            end
        end
    else
        self.name = "Enable MultiInput"
        self.handler.passKeyToKeybinds = savedKeyInput
        savedKeyInput = nil
    end
end

return MyKey