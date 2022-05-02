local s = require "ShortcutDropdown"

local MyKey = s:new()

MyKey.key = "l"

MyKey.listToPass = {}

function MyKey:onActivate()
    self:findFiles()
    self:startText("", "What map to load?", true)
end

function MyKey:onReciveText(text)
    if self:contains(text) then
        loadFile(text)
        print("a")
        return
    end
    self:startText(text, "What map to load?", true)
end

function getPath()
    local sys = love.system.getOS()
    if sys == "Windows" then
        return "../bin/Debug/netcoreapp3.1/maps/"
    elseif sys == "Linux" then
        return "../maps/"
    end
end

function getCMD()
    local sys = love.system.getOS()
    if sys == "Windows" then
        return 'dir "'..getPath()..'" /b'
    elseif sys == "Linux" then
        return 'ls "'..getPath()..'"'
    end
end

function getGmatch()
   return "(.-).map"
end

function MyKey:findFiles()
    self.listToPass = {}

    local i, t = 0, ""
    local pfile = io.popen(getCMD())
    for str in pfile:lines() do
        i = i + 1
        t = t .. str
    end
    pfile:close()
    i = 0

    for strPart in string.gmatch(t, getGmatch()) do
        table.insert(self.listToPass, strPart)
    end
end

function loadFile(loadFileName)
    print(loadFileName)
    fileName = loadFileName
    local f = io.open(getPath()..fileName..".map", "r")
    local lines = {}
    for line in f:lines() do
        table.insert(lines, line)
    end
    local nrsL = string.numsplit(lines[1], " ")
    grid = newGrid(nrsL[1], nrsL[2])
    
    nrsL = string.numsplit(lines[2], " ")
    spawn = {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1)}
    nrsL = string.numsplit(lines[3], " ")
    spawnLook = {-nrsL[1], nrsL[2]}

    local count = 0
    for y = 1,gH do
        for x = gW,1,-1 do
            local layers = string.split(lines[4+count], " ")
            local layers1 = string.split(layers[1], "/")
            local layers2 = string.split(layers[2], "/")
            local layers3 = string.split(layers[3], "/")

            grid[1][x][y] = {"", {layers1[2] or "", layers1[3] or ""}}
            grid[2][x][y] = {"", {layers2[2] or "", layers2[3] or ""}}
            grid[3][x][y] = {"", {layers3[2] or "", layers3[3] or ""}}
            count = count + 1
        end
    end

    sprites = {}
    for i = count+4, #lines do
        nrsL = string.split(lines[i], " ")
        local nrsL2 = string.split(nrsL[3], "-")

        local imgs = {}
        for _, path in pairs(nrsL2) do
            local nrsL3 = string.split(path, "/")
            table.insert(imgs, {nrsL3[2], nrsL3[3]})
        end
        

        if #nrsL == 3 then
            table.insert(sprites, {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1), imgs, ""})
        else
            local str = ""
            for i2 = 4, #nrsL do
                str = str..nrsL[i2].." "
            end
            str = string.sub(str, 0, math.max(0, #str-1))
            
            table.insert(sprites, {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1), imgs, str})
        end
    end
end

return MyKey
