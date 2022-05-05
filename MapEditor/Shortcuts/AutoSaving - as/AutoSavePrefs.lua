local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "p"

local doSave = false
local updateLoop = nil
local timer = 0
local timeBetweenSaving = 1

function MyKey:onActivate()
    doSave = not doSave
    self.name = "AutoSavePrefs - off"
    if doSave then
        self.name = "AutoSavePrefs - on"
    end
end

function MyKey:setup()
    updateLoop = love.graphics.update
    love.graphics.update = function (dt)
        updateLoop(dt)
        timer = timer + dt
        if timer > timeBetweenSaving then
            timer = timer - timeBetweenSaving
            self.handler.savePref()
        end
    end
end

return MyKey