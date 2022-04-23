local love = love

love.graphics.setDefaultFilter("nearest", "nearest")

local socket = require("socket")
local directory = {}

function loadImage(path)
    local str, len = love.filesystem.read(path)

    str = string.gsub(str, "#[^\r\n]+\r?\n", "")

    local _,start, w, h, colorMax = string.find(str, "(%d+) (%d+)\r?\n(%d+)")
    local iData = love.image.newImageData(tonumber(w), tonumber(h))
    colorMax = tonumber(colorMax)

    local i = 0
    local thisColor = {}
    for strPart in string.gmatch(str, "(%d+)") do
        if i > 3 then
            local colorPos = ((i-1)%3)+1
            thisColor[colorPos] = tonumber(strPart)/colorMax
            if i%3 == 0 then
                local pos = math.ceil(i/3)-2
                local a = 1
                if thisColor[1]==0 and thisColor[2]==0 and thisColor[3]==0 then
                    a = 0
                end
                iData:setPixel(pos-math.floor(pos/w)*w, math.floor(pos/w), thisColor[1], thisColor[2], thisColor[3], a)
            end
        end
        i = i + 1
    end

    return love.graphics.newImage(iData)
end

local globalSpriteIndexHelper = 1;

function string.numsplit(s, delimiter)
    local result = {}
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, tonumber(match))
    end
    return result
end

function string.split(s, delimiter)
    local result = {}
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, match)
    end
    return result
end

-- load all files in the img folder
local namesOfFiles = love.filesystem.getDirectoryItems("img")
local folders = {}

for _, fileName in pairs(namesOfFiles) do
    if love.filesystem.getInfo("img/"..fileName).type == "directory" then
        local namesOfFiles2 = love.filesystem.getDirectoryItems("img/"..fileName)
        folders[fileName] = {}
        for _, fileName2 in pairs(namesOfFiles2) do
            if string.sub(fileName2, #fileName2-3, #fileName2) == ".ppm" then
                folders[fileName][fileName2] = {loadImage("img/"..fileName.."/"..fileName2), "img/"..fileName.."/"..fileName2, {fileName, fileName2}}
            end
        end
    end
end

-- declare variables
local w, h = love.graphics.getWidth(), love.graphics.getHeight()
local grid = {}
local gW, gH = 20, 20
local gridLayer = 2
local gridOffsetX, gridOffsetY = 0, 0
local mx, my = -1, -1
local scale = 10
local px, py = 0, 0
local selectedForMenuX, selectedForMenuY = 0, 0
local openMen = false

local selected = {"", ""}

local cloneSave = {0, 0, ""}

local guiTileSize = 40
local guiTilediff = 6
local guiMaxTiles = 17

local gridActive = false;

local spawn = {0, 0}
local spawnLook = {0, 0}
local spawnPlacing = 1

local directoryName = ""

local editingSprite = 0
local sprites = {}

local sys = "Win"
local fileName = "newMap"
local setSize = "20 20"

local definingSize = false

local editingFName = false
local ignoreNr = 0

local keys = "1234567890-"
local keysSize = "1234567890- "
local txtKeys = "abcdefghijklmnopqrstuvwxyz"

local animations = {

}

function newGrid(gWin, gHin)
    local grid = {}
    gW, gH = gWin, gHin
    for i = 1,3 do
        grid[i] = {}
        for x = 1,gW do
            grid[i][x] = {}
            for y = 1,gH do
                grid[i][x][y] = {"", {"", ""}}
            end
        end
    end
    
    return grid
end

local multiSelect = {}

grid = newGrid(20, 20)

function love.load()
    grid = newGrid(gW, gH)
end

function love.draw()
    -- layer of the grid
    local grid = grid[gridLayer]

    -- since we scale a lot, the lines will need to be reaaal small
    love.graphics.setLineWidth(0.02)

    -- get mouse position
    local mX, mY = love.mouse.getPosition()

    -- do this for making screen drag work
    if mx == -1 then
        mX, mY = mx, my
    end

    -- translate everything so when it gets drawn, it gets drawn in the right way
    -- also scale it...
    love.graphics.translate(gridOffsetX+(mX-mx)+w/2, gridOffsetY+(mY-my)+h/2)
    love.graphics.scale(scale, scale)

    -- draw the whole grid, and images in it if needed
    for x = -gW/2+1,gW/2 do
        for y = -gH/2+1,gH/2 do
            local thisFile = grid[x+gW/2][y+gH/2]
            if thisFile[2][1] == "" then
                love.graphics.setColor(0.6, 0.6, 0.6)
                love.graphics.rectangle("fill", x, y, 1, 1)
            elseif thisFile[2][1] ~= "" then
                local thisImg = folders[thisFile[2][1]][thisFile[2][2]]
                
                love.graphics.setColor(1, 1, 1)
                love.graphics.draw(thisImg[1], x, y, 0, 1/thisImg[1]:getWidth(), 1/thisImg[1]:getHeight())
            end
            love.graphics.setColor(1, 1, 1)
            love.graphics.rectangle("line", x, y, 1, 1)
        end
    end

    love.graphics.circle("fill", spawn[1], spawn[2], 0.3)
    love.graphics.setColor(1, 1, 1, 1)
    love.graphics.line(spawn[1], spawn[2], spawn[1]+spawnLook[1], spawn[2]+spawnLook[2])
    love.graphics.setColor(0, 0, 0, 1)
    love.graphics.line(spawn[1]+spawnLook[1]/3, spawn[2]+spawnLook[2]/3, spawn[1]+spawnLook[1]/3*2, spawn[2]+spawnLook[2]/3*2)

    love.graphics.setColor(1, 1, 1, 1)
    if openMen then
        love.graphics.rectangle("fill", px-gW/2+1, py-gH/2+1, 0.5, 0.8)
    end

    for _, sprite in pairs(sprites) do
        local roundDown = math.floor(globalSpriteIndexHelper)
        local maxSprites = #sprite[3]
        local thisIndex = roundDown%maxSprites+1
        
        local thisImg = folders[sprite[3][thisIndex][1]][sprite[3][thisIndex][2]]

        love.graphics.draw(thisImg[1], sprite[1]-0.3, sprite[2]-0.3, 0, 0.6/thisImg[1]:getWidth(), 0.6/thisImg[1]:getHeight())
    end
    
    love.graphics.origin()
    love.graphics.setLineWidth(2)

    if editingSprite ~= 0 then
        love.graphics.setColor(0, 0, 0, 0.2)
        love.graphics.rectangle("fill", 0, 0, w, h)

        love.graphics.setColor(1, 1, 1, 1)
        love.graphics.rectangle("fill", 0, h/2-10, w, 20)
        love.graphics.setColor(0, 0, 0)
        love.graphics.printf(sprites[editingSprite][4], 0, h/2-6, w, "center")
    else
        if directoryName == "" then
            local i = 0
            for _, folderName in pairs(folders) do
                i = i + 1

                love.graphics.setColor(0.6, 0.6, 0.6)
                love.graphics.rectangle("fill", 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1, 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1, (guiTileSize+guiTilediff/2),  (guiTileSize+guiTilediff/2))
                
                love.graphics.setColor(0, 0, 0)
                love.graphics.printf(_, 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles), 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1), (guiTileSize+guiTilediff/2), "center")
            end
        else
            local i = 0
            for _, imageNPath in pairs(folders[directoryName]) do
                i = i + 1

                love.graphics.setColor(1, 1, 1)

                for _, selected in pairs(multiSelect) do
                    if selected[1] == imageNPath[3][1] and selected[2] == imageNPath[3][2] then
                        love.graphics.setColor(1, 1, 0)
                    end
                end

                love.graphics.rectangle("fill", 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1, 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1, (guiTileSize+guiTilediff/2),  (guiTileSize+guiTilediff/2))
                
                love.graphics.setColor(1, 1, 1)
                love.graphics.draw(imageNPath[1], 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles), 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1), 0, guiTileSize/imageNPath[1]:getWidth(), guiTileSize/imageNPath[1]:getHeight())
            end
        end
    end

    love.graphics.setColor(1, 1, 1)
    love.graphics.print("[n] Size: "..setSize, 5, h-125)
    love.graphics.print("[z] Sys: "..sys, 5, h-105)

    if editingFName then
        love.graphics.setColor(0.6, 0.6, 0.6)
        love.graphics.print("[x] File name: "..findMachFile(), 5, h-85)
    end

    love.graphics.setColor(1, 1, 1)

    love.graphics.print("[x] File name: "..fileName, 5, h-85)
    if gridActive then
        love.graphics.print("[g] Grid: active", 5, h-65)
    else
        love.graphics.print("[g] Grid: not active", 5, h-65)
    end
    
    for i = 1,3 do
        love.graphics.setColor(0.6, 0.6, 0.6, 0.4)
        if gridLayer == i then
            love.graphics.setColor(0.8, 0.8, 0.8, 0.8)
        end
        love.graphics.rectangle("fill", w-100, h-60-20*(i-1), 80, 40)
    end

    love.graphics.setColor(1, 1, 1, 1)
    local Tmx, Tmy = love.mouse.getPosition()
    local px, py = math.abs((((Tmx-w/2-gridOffsetX)/scale)+gW/2-1)-gW), (((Tmy-h/2-gridOffsetY)/scale)+gH/2-1)
    local pointX, pointY = math.floor(px), math.floor(py)
    love.graphics.print(px .. " | " .. py, 5, h-145)
    love.graphics.print(pointX .. " | " .. pointY, 5, h-165)
end

function love.keypressed(key)
    if definingSize == true then
        if key == "space" then key = " " end
        if key == "backspace" then
            setSize = string.sub(setSize, 0, #setSize-1)
        elseif key == "return" then
            local wh = string.numsplit(setSize, " ")
            if wh[1] and wh[2] then
                grid = newGrid(wh[1], wh[2])
                definingSize = false
            end
        end
        for i = 1,#keysSize do
            if key == string.sub(keysSize, i, i) then
                setSize = setSize .. key
            end
        end
        return
    end
    if editingFName == true then
        if key == "backspace" then
            fileName = string.sub(fileName, 0, #fileName-1)
        elseif key == "return" then
            editingFName = false
            ignoreNr = 0
        elseif key == "tab" then
            fileName = findMachFile()
        elseif key == "up" then
            ignoreNr = math.max(ignoreNr - 1, 0)
        elseif key == "down" then
            ignoreNr = ignoreNr + 1
        end
        for i = 1,#txtKeys do
            if key == string.sub(txtKeys, i, i) then
                if love.keyboard.isDown("lshift") then
                    fileName = fileName .. string.upper(key)
                else
                    fileName = fileName .. key
                end
            end
        end
        
        while findMachFile() == "" and (ignoreNr ~= 0) do
            ignoreNr = ignoreNr - 1
        end
        return
    end
    if key == "f" then
        for x = 1,gW do
            for y = 1,gH do
                grid[gridLayer][x][y][2] = selected
            end
        end
    elseif key == "z" then
        if sys == "Win" then
            sys = "Lin"
        else
            sys = "Win"
        end
    elseif key == "x" then
        editingFName = true
    elseif key == "escape" then
        directoryName = ""
        selected = {"", ""}
    elseif key == "up" then
        gridLayer = math.min(gridLayer+1, 3)
    elseif key == "down" then
        gridLayer = math.max(gridLayer-1, 1)
    elseif key == "e" then
        local mx, my = love.mouse.getPosition()
        px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)

        local closest = 0
        local distance = 0.5
        for i,sprite in ipairs(sprites) do
            local thisDist = math.dist(sprite[1], sprite[2], px, py)
            if thisDist < distance then
                distance = thisDist
                closest = i
            end
        end

        if closest ~= 0 then
            editingSprite = closest
        end
    elseif key == "c" then
        local mx, my = love.mouse.getPosition()
        px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)

        local closest = 0
        local distance = 0.5
        for i,sprite in ipairs(sprites) do
            local thisDist = math.dist(sprite[1], sprite[2], px, py)
            if thisDist < distance then
                distance = thisDist
                closest = i
            end
        end

        if closest ~= 0 then
            if cloneSave[1] ~= closest then
                cloneSave[1] = closest
                cloneSave[2] = sprites[closest][3]
                cloneSave[3] = sprites[closest][4]
            else
                cloneSave[1] = 0
            end
        end
    elseif key == "b" then
        local mx, my = love.mouse.getPosition()
        px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)

        if cloneSave[1] ~= 0 then
            if gridActive then
                table.insert(sprites, {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2, cloneSave[2], cloneSave[3]})
            else
                table.insert(sprites, {px, py, cloneSave[2], cloneSave[3]})
            end
        end
    elseif key == "s" then
        saveFile()
    elseif key == "n" then
        sprites = {}
        setSize = ""
        definingSize = true
    elseif key == "g" then
        gridActive = not gridActive;
    elseif key == "l" then
        loadFile()
    elseif key == "p" then
        local mx, my = love.mouse.getPosition()
        px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)
        if spawnPlacing == 1 then
            if gridActive then
                spawn = {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2}
            else
                spawn = {px, py}
            end
            spawnPlacing = 2
        else
            local v = math.floor(math.atan2(py-spawn[2], px-spawn[1])/(math.pi/2)+math.pi/8)*(math.pi/2)
            spawnLook = {math.cos(v), math.sin(v)}
            spawnPlacing = 1
        end
    else
        if key == "backspace" then
            if editingSprite ~= 0 then
                local newStrList = string.split(sprites[editingSprite][4], " ")
                sprites[editingSprite][4] = ""

                for i = 1,#newStrList-1 do
                    if i == 1 then
                        sprites[editingSprite][4] = newStrList[i]
                    else
                        sprites[editingSprite][4] = sprites[editingSprite][4].." "..newStrList[i]
                    end
                end
            end
        elseif key == "return" then
            editingSprite = 0
        elseif key == "delete" then
            table.remove(sprites, editingSprite)
            editingSprite = 0
        end
        if editingSprite ~= 0 then
            for i = 1,#keys do
                if key == string.sub(keys, i, i) then
                    if #sprites[editingSprite][4] == 0 then
                        sprites[editingSprite][4] = sprites[editingSprite][4] .. key
                    else
                        if love.keyboard.isDown("lshift") then
                            sprites[editingSprite][4] = sprites[editingSprite][4] .. key
                        else
                            sprites[editingSprite][4] = sprites[editingSprite][4] .. " " .. key
                        end
                    end
                end
            end
        end
    end
end

function love.mousepressed(x, y, b)
    if b == 1 and y < 2+(guiTileSize+guiTilediff)*(math.ceil(getLenOfCurImage()/guiMaxTiles)) then
        for i = 1, getLenOfCurImage() do
            if x > 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 and x < 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 + (guiTileSize+guiTilediff/2) then
                if y > 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 and y < 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 + (guiTileSize+guiTilediff/2) then
                    if directoryName == "" then
                        local i2 = 0;
                        for folderName, _ in pairs(folders) do
                            i2 = i2 + 1
                            if i2 == i then
                                directoryName = folderName
                            end
                        end
                    else
                        local i2 = 0;
                        for dirName, _  in pairs(folders[directoryName]) do
                            i2 = i2 + 1
                            if i2 == i then
                                if love.keyboard.isDown("lshift") then
                                    table.insert(multiSelect, {directoryName, dirName})
                                else
                                    multiSelect = {}
                                    table.insert(multiSelect, {directoryName, dirName})
                                    selected = {directoryName, dirName}
                                end
                            end
                        end
                    end
                end
            end
        end
        my = -2
    elseif b == 3 then -- clone this tile to selected
        local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
        if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
            directoryName = grid[gridLayer][pointX][pointY][2][1]
            selected = grid[gridLayer][pointX][pointY][2]
        end
    else
        if b == 1 and not love.keyboard.isDown("space") then
            mx, my = x, y
        elseif love.keyboard.isDown("space") then
            my = -2
        end
    end
end

function love.mousereleased(x, y, b)
    local grid = grid[gridLayer]
    openMen = false
    if b == 1 then
        if my ~= -2 then
            gridOffsetX, gridOffsetY = gridOffsetX+(x-mx), gridOffsetY+(y-my)
            if mx-x == 0 and my-y == 0 and selected[1] ~= "" then
                local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
                if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
                    placeAt(pointX, pointY)
                end
            end
        end
        mx, my = -1, -1
    elseif b == 2 then
        px, py = ((x-w/2-gridOffsetX)/scale), ((y-h/2-gridOffsetY)/scale)

        if selected[1] ~= "" then
            if gridActive then
                table.insert(sprites, {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2, {}, ""})
            else
                table.insert(sprites, {px, py, {}, ""})
            end

            for i, selected in ipairs(multiSelect) do
                sprites[#sprites][3][i] = selected
            end
        end
    end
end

function love.wheelmoved(x, y)
    local mx, my = love.mouse.getPosition()
    scale = scale + y
end

function love.update(dt)
    globalSpriteIndexHelper = globalSpriteIndexHelper + dt;
    local grid = grid[gridLayer]
    local x, y = love.mouse.getPosition()
    if love.keyboard.isDown("space") and love.mouse.isDown(1) then
        local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
        if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
            placeAt(pointX, pointY)
        end
    end
end

function placeAt(x, y)
    if selected[1] ~= "" then
        if love.keyboard.isDown("lshift") then
            grid[gridLayer][x][y][2] = {"", ""}
        else
            grid[gridLayer][x][y][2] = selected

            --[[
            if #multiSelect == 1 then
                grid[gridLayer][x][y][2] = selected
            else
                grid[gridLayer][x][y][2] = {}
                for i, selected in ipairs(multiSelect) do
                    grid[gridLayer][x][y][2][i] = selected
                end


            end
            ]]-- we dont have animated wall ground and roof textures, lol...
        end
    else
        grid[gridLayer][x][y][2] = {"", ""}
    end
end

function math.dist(x1,y1, x2,y2) return ((x2-x1)^2+(y2-y1)^2)^0.5 end

function gridMakePath()
    for y = 1,gH do
        for x = gW,1,-1 do
            for i = 1,3 do
                if grid[i][x][y][2][1] ~= "" then
                    grid[i][x][y][1] = folders[grid[i][x][y][2][1]][grid[i][x][y][2][2]][2]
                else
                    grid[i][x][y][1] = "";
                end
            end
        end
    end
end

function saveFile()
    gridMakePath()

    local str = ""
    str = str..gW.." "..gH.."\n"

    str = str..math.abs((spawn[1]+gW/2-1)-gW).." "..(spawn[2]+gH/2-1).."\n"
    str = str..(-spawnLook[1]).." "..spawnLook[2].."\n"

    for y = 1,gH do
        for x = gW,1,-1 do
            for i = 1,3 do
                str = str..grid[i][x][y][1]
                if i == 3 then
                    str = str.."\n"
                else
                    str = str.." "
                end
            end
        end
    end

    for _, sprite in pairs(sprites) do
        if sprite[4] == "" then
            str = str..math.abs((sprite[1]+gW/2-1)-gW).." "..(sprite[2]+gH/2-1).." "..pathListToString(sprite[3]).."\n"
        else
            str = str..math.abs((sprite[1]+gW/2-1)-gW).." "..(sprite[2]+gH/2-1).." "..pathListToString(sprite[3]).." "..sprite[4].."\n"
        end
    end

    str = string.sub(str, 0, #str-1)

    local f = io.open(getPath()..fileName..".map", "w")
    f:write(str)
    f:close()
end

function pathListToString(list) -- for sprites
    local str = ""
    for _, path in pairs(list) do
        str = str..folders[path[1]][path[2]][2].."-"
    end
    str = string.sub(str, 0, #str-1)
    return str
end

function getPath()
    if sys == "Win" then
        return "bin/Debug/netcoreapp3.1/maps/"
    elseif sys == "Lin" then
        return "../maps/"
    end
end

function getCMD()
    if sys == "Win" then
        return 'dir "bin/Debug/netcoreapp3.1/maps/"'
    elseif sys == "Lin" then
        return 'ls "../maps/"'
    end
end

function getGmatch()
    if sys == "Win" then
        return "%d ([^%d]*).map[^/.]"
    elseif sys == "Lin" then
        return "(%a+).map"
    end
end

function findMachFile()
    local i, t = 0, ""
    local pfile = io.popen(getCMD())
    for str in pfile:lines() do
        i = i + 1
        t = t .. str
    end
    pfile:close()
    i = 0

    for strPart in string.gmatch(t, getGmatch()) do
        if string.sub(strPart, 0, #fileName) == fileName then
            i = i + 1
            if i > ignoreNr then
                return strPart
            end
        end
    end

    return ""
end

function getLenOfCurImage()
    local len = 0

    if directoryName == "" then
        len = lenOfPAIRSList(folders)
    else
        len = lenOfPAIRSList(folders[directoryName])
    end

    return len
end

function lenOfPAIRSList(list)
    local len = 0
    for i, v in pairs(list) do
        len = len + 1
    end
    return len
end

function loadFile()
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