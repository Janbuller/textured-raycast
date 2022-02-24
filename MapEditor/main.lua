local love = love

local socket = require("socket")

function loadImage(path)
    local str, len = love.filesystem.read(path)

    str = string.gsub(str, "#[^\r\n]+\r\n?\n?", "")

    local _,start, w, h, colorMax = string.find(str, "(%d+) (%d+)\r\n?\n?(%d+)")
    local iData = love.image.newImageData(tonumber(h), tonumber(w))
    colorMax = tonumber(colorMax)

    local i = 0
    local thisColor = {}
    for strPart in string.gmatch(str, "(%d+)") do
        if i > 3 then
            local colorPos = ((i-1)%3)+1
            thisColor[colorPos] = tonumber(strPart)/colorMax
            if i%3 == 0 then
                local pos = math.ceil(i/3)-2
                iData:setPixel(pos-math.floor(pos/w)*w, math.floor(pos/w), thisColor[1], thisColor[2], thisColor[3])
            end
        end
        i = i + 1
    end

    return love.graphics.newImage(iData)
end

function string.numsplit(s, delimiter)
    result = {};
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, tonumber(match));
    end
    return result;
end

local image = {
    "img/wolfenstein/greystone.ppm",
    "img/wolfenstein/redbrick.ppm",
    "img/wolfenstein/bluestone.ppm",
    "img/test5.ppm",
    "img/wolfenstein/redstone.ppm",
    "img/wolfenstein/colorstone.ppm",
    "img/wolfenstein/pillar.ppm",
    "img/wolfenstein/barrel.ppm",
    "img/wolfenstein/greenlight.ppm",
    "img/wolfenstein/barrelBroken.ppm",
    "img/shadyman.ppm",
    "img/button.ppm",
}
local images = #image

for i, path in ipairs(image) do
	love.graphics.clear()
    
    love.graphics.setColor(0.6, 0.6, 0.6)
    love.graphics.rectangle("fill", 10, 10, 200, 40)
    love.graphics.setColor(1, 1, 1)
    love.graphics.rectangle("fill", 10, 10, 200/images*i, 40)
    love.graphics.setColor(0, 0, 0)
    love.graphics.rectangle("line", 10, 10, 200, 40)
    love.graphics.print(i.."/"..images, 15, 15)

	love.graphics.present()
    image[i] = loadImage(path)
end

local w, h = love.graphics.getWidth(), love.graphics.getHeight()
local grid = {}
local gW, gH = 20, 20
local gridOffsetX, gridOffsetY = 0, 0
local mx, my = -1, -1
local scale = 10
local px, py = 0, 0
local selectedForMenuX, selectedForMenuY = 0, 0
local openMen = false

local selected = 1
local drawSelect = true

local guiTileSize = 40
local guiTilediff = 6
local guiMaxTiles = 30

local spawn = {0, 0}
local spawnLook = {0, 0}
local spawnPlacing = 1
local floor = 1
local roof = 0

local editingSprite = 0
local sprites = {}

local sys = "Win"
local fileName = "newMap"
local setSize = ""

local definingSize = true

local editingFName = false

local keys = "1234567890"
local keysSize = "1234567890 "
local txtKeys = "abcdefghijklmnopqrstuvwxyz"

function newGrid(gWin, gHin)
    local grid = {}
    gW, gH = gWin, gHin

    for x = 1,gW do
        grid[x] = {}
        for y = 1,gH do
            grid[x][y] = 0
        end
    end

    return grid
end

function love.load()
    grid = newGrid(gW, gH)
end

function love.draw()
    love.graphics.setLineWidth(0.02)
    local mX, mY = love.mouse.getPosition()
    if mx == -1 then
        mX, mY = mx, my
    end
    love.graphics.translate(gridOffsetX+(mX-mx)+w/2, gridOffsetY+(mY-my)+h/2)
    love.graphics.scale(scale, scale)

    for x = -gW/2+1,gW/2 do
        for y = -gH/2+1,gH/2 do
            if grid[x+gW/2][y+gH/2] == -1 then
                love.graphics.setColor(0.6, 0.6, 0.6)
                love.graphics.rectangle("fill", x, y, 1, 1)
            elseif grid[x+gW/2][y+gH/2] ~= 0 then
                love.graphics.setColor(1, 1, 1)
                love.graphics.draw(image[grid[x+gW/2][y+gH/2]], x, y, 0, 1/image[grid[x+gW/2][y+gH/2]]:getWidth())
            end
            love.graphics.setColor(1, 1, 1)
            love.graphics.rectangle("line", x, y, 1, 1)
        end
    end

    love.graphics.circle("fill", spawn[1], spawn[2], 0.3)
    love.graphics.line(spawn[1], spawn[2], spawn[1]+spawnLook[1], spawn[2]+spawnLook[2])

    love.graphics.setColor(1, 1, 1, 1)
    if openMen then
        love.graphics.rectangle("fill", px-gW/2+1, py-gH/2+1, 0.5, 0.8)
    end

    for _, sprite in pairs(sprites) do
        love.graphics.draw(image[sprite[3]], sprite[1]-0.3, sprite[2]-0.3, 0, 0.6/image[sprite[3]]:getWidth(), 0.6/image[sprite[3]]:getHeight())
    end
    
    love.graphics.origin()
    love.graphics.setLineWidth(2)

    love.graphics.draw(image[floor], 5, h-45, 0, 40/image[floor]:getWidth(), 40/image[floor]:getHeight())
    
    if roof ~= 0 then
        love.graphics.draw(image[roof], 50, h-45, 0, 40/image[roof]:getWidth(), 40/image[roof]:getHeight())
    end

    if editingSprite ~= 0 then
        love.graphics.setColor(0, 0, 0, 0.2)
        love.graphics.rectangle("fill", 0, 0, w, h)

        love.graphics.setColor(1, 1, 1, 1)
        love.graphics.rectangle("fill", 0, h/2-10, w, 20)
        love.graphics.setColor(0, 0, 0)
        love.graphics.printf(sprites[editingSprite][4], 0, h/2-6, w, "center")
    else
        if drawSelect then
            for i = 1,images do
                love.graphics.setColor(1, 1, 1)
                if selected == i then
                    love.graphics.setColor(1, 1, 0)
                end
                love.graphics.rectangle("fill", 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1, 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1, (guiTileSize+guiTilediff/2),  (guiTileSize+guiTilediff/2))
                
                love.graphics.setColor(1, 1, 1)
                love.graphics.draw(image[i], 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles), 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1), 0, guiTileSize/image[i]:getWidth(), guiTileSize/image[i]:getHeight())
            end
        end
    end

    love.graphics.print("[n] Size: "..setSize, 5, h-125)
    love.graphics.print("[z] Sys: "..sys, 5, h-105)
    love.graphics.print("[x] File name: "..fileName, 5, h-85)
    
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
        return
    end
    if key == "m" then
        drawSelect = not drawSelect
    elseif key == "f" then
        floor = selected
    elseif key == "r" then
        if roof == selected then
            roof = 0
        else
            roof = selected
        end
    elseif key == "z" then
        if sys == "Win" then
            sys = "Lin"
        else
            sys = "Win"
        end
    elseif key == "x" then
        editingFName = true
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
    elseif key == "s" then
        saveFile()
    elseif key == "n" then
        setSize = ""
        definingSize = true
    elseif key == "l" then
        loadFile()
    elseif key == "p" then
        local mx, my = love.mouse.getPosition()
        px, py = ((mx-w/2-gridOffsetX)/scale), ((my-h/2-gridOffsetY)/scale)
        if spawnPlacing == 1 then
            spawn = {px, py}
            spawnPlacing = 2
        else
            local v = math.floor(math.atan2(py-spawn[2], px-spawn[1])/(math.pi/2)+math.pi/8)*(math.pi/2)
            spawnLook = {math.floor(math.cos(v)), math.floor(math.sin(v))}
            spawnPlacing = 1
        end
    else
        if key == "backspace" then
            sprites[editingSprite][4] = string.sub(sprites[editingSprite][4], 0, math.max(#sprites[editingSprite][4]-2, 0))
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
                        sprites[editingSprite][4] = sprites[editingSprite][4] .. " " .. key
                    end
                end
            end
        end
    end
end

function love.mousepressed(x, y, b)
    if b == 1 and drawSelect and y < 2+(guiTileSize+guiTilediff)*(math.ceil(images/guiMaxTiles)) then
        for i = 1,images do
            if x > 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 and x < 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 + (guiTileSize+guiTilediff/2) then
                if y > 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 and y < 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 + (guiTileSize+guiTilediff/2) then
                    selected = i
                end
            end
        end
        my = -2
    else
        if b == 1 and not love.keyboard.isDown("space") then
            mx, my = x, y
        elseif love.keyboard.isDown("space") then
            my = -2
        end
    end
end

function love.mousereleased(x, y, b)
    openMen = false
    if b == 1 then
        if my ~= -2 then
            gridOffsetX, gridOffsetY = gridOffsetX+(x-mx), gridOffsetY+(y-my)
            if mx-x == 0 and my-y == 0 then
                local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
                if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
                    if love.keyboard.isDown("lshift") then
                        grid[pointX][pointY] = -1
                    else
                        grid[pointX][pointY] = 0
                    end
                end
            end
        end
        mx, my = -1, -1
    elseif b == 2 then
        px, py = ((x-w/2-gridOffsetX)/scale), ((y-h/2-gridOffsetY)/scale)

        table.insert(sprites, {px, py, selected, ""})
    end
end

function love.wheelmoved(x, y)
    local mx, my = love.mouse.getPosition()
    scale = scale + y
end

function love.update()
    local x, y = love.mouse.getPosition()
    if love.keyboard.isDown("space") and love.mouse.isDown(1) then
        local pointX, pointY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
        if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
            grid[pointX][pointY] = selected
        end
    end
end

function math.dist(x1,y1, x2,y2) return ((x2-x1)^2+(y2-y1)^2)^0.5 end

function saveFile()
    local str = ""

    if roof == 0 then
        str = str..gW.." "..gH.." "..floor.."\n"
    else
        str = str..gW.." "..gH.." "..floor.." "..roof.."\n"
    end
    str = str..math.abs((spawn[1]+gW/2-1)-gW).." "..(spawn[2]+gH/2-1).."\n"
    str = str..spawnLook[1].." "..spawnLook[2].."\n"

    for y = 1,gH do
        for x = gW,1,-1 do
            str = str..grid[x][y].."\n"
        end
    end

    for _, sprite in pairs(sprites) do
        if sprite[4] == "" then
            str = str..math.abs((sprite[1]+gW/2-1)-gW).." "..(sprite[2]+gH/2-1).." "..sprite[3].."\n"
        else
            str = str..math.abs((sprite[1]+gW/2-1)-gW).." "..(sprite[2]+gH/2-1).." "..sprite[3].." "..sprite[4].."\n"
        end
    end

    str = string.sub(str, 0, #str-1)

    local f = io.open(getPath()..fileName..".map", "w")
    f:write(str)
    f:close()
end

function getPath()
    if sys == "Win" then
        return "bin/Debug/netcoreapp3.1/maps/"
    elseif sys == "Lin" then
        return "../maps/"
    end
end

function loadFile()
    local f = io.open(getPath()..fileName..".map", "r")
    local lines = {}
    for line in f:lines() do
        table.insert(lines, line)
    end
    local nrsL = string.numsplit(lines[1], " ")
    grid = newGrid(nrsL[1], nrsL[2])
    floor = nrsL[3]
    if nrsL[4] then
        roof = nrsL[4]
    end
    
    nrsL = string.numsplit(lines[2], " ")
    spawn = {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1)}
    nrsL = string.numsplit(lines[3], " ")
    spawnLook = {nrsL[1], nrsL[2]}

    local count = 0
    for y = 1,gH do
        for x = gW,1,-1 do
            grid[x][y] = tonumber(lines[4+count])
            count = count + 1
        end
    end

    sprites = {}
    for i = count+4, #lines do
        nrsL = string.numsplit(lines[i], " ")
        if #nrsL == 3 then
            table.insert(sprites, {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1), nrsL[3], ""})
        else
            local str = ""
            for i2 = 4, #nrsL do
                str = str..nrsL[i2].." "
            end
            str = string.sub(str, 0, math.max(0, #str-1))
            
            table.insert(sprites, {(math.abs(nrsL[1]-gW)-gW/2+1), (nrsL[2]-gH/2+1), nrsL[3], str})
        end
    end
end