local s = require "Shortcut"
local json = require "dkjson"

local MyKey = s:new()

MyKey.key = "p"

local doSave = false
local updateLoop = nil
local timer = 0
local timeBetweenSaving = 1

function MyKey:onActivate()
    doSave = not doSave
    self.name = "AutoSaveMap - off"
    if doSave then
        self.name = "AutoSaveMap - on"
    end
end

function MyKey:setup()
    print("ISDJAOSLÆDJASLKDJASKLDJASOÆLKDJAS")
    local f = io.open("AUTOSAVE", "r")
    print(f, "a")

    updateLoop = love.graphics.update
    love.graphics.update = function (dt)
        updateLoop(dt)
        timer = timer + dt
        if timer > timeBetweenSaving then
            timer = timer - timeBetweenSaving
            local f = io.open("AUTOSAVE", "w")
            f:write(json.encode(sprites, grid))
            f:close()
        end
    end
end

return MyKey