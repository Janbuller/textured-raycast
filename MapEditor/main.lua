local SCMan = require("ShortcutHandler")

love.graphics.setDefaultFilter("nearest", "nearest")

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

globalSpriteIndexHelper = 1;

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
folders = {}

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

transform = love.math.newTransform()

-- declare variables
w, h = love.graphics.getWidth(), love.graphics.getHeight()
grid = {}
gW, gH = 20, 20
gridLayer = 2
gridOffsetX, gridOffsetY = 0, 0
mx, my = -1, -1
scale = 20

selected = {"", ""}

guiTileSize = 40
guiTilediff = 6
guiMaxTiles = 17

gridActive = false

spawn = {0, 0}
spawnLook = {0, 0}

directoryName = ""

editingSprite = 0
sprites = {}

fileName = "newMap"

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

multiSelect = {}

grid = newGrid(20, 20)

function love.load()
    SCMan.load();
    grid = newGrid(gW, gH)
end

function love.textinput(t)
   SCMan.textinput(t);
end

function love.draw()
    love.graphics.setBackgroundColor(SCMan.colors["AllBackground"])
    -- layer of the grid
    local grid = grid[gridLayer]

    -- since we scale a lot, the lines will need to be reaaal small
    love.graphics.setLineWidth(1/scale)

    -- get mouse position
    local mX, mY = love.mouse.getPosition()

    -- do this for making screen drag work
    if mx == -1 then
        mX, mY = mx, my
    end

    -- translate everything so when it gets drawn, it gets drawn in the right way
    -- also scale it...
    transform = love.math.newTransform():translate(w/2, h/2):scale(scale, scale):translate(gridOffsetX+(mX-mx)/scale, gridOffsetY+(mY-my)/scale)
    love.graphics.applyTransform(transform)

    -- draw the whole grid, and images in it if needed
    for x = -gW/2+1,gW/2 do
        for y = -gH/2+1,gH/2 do
            local thisFile = grid[x+gW/2][y+gH/2]
            if thisFile[2][1] == "" then
                love.graphics.setColor(SCMan.colors["GridBackground"])
                love.graphics.rectangle("fill", x, y, 1, 1)
            elseif thisFile[2][1] ~= "" then
                local thisImg = folders[thisFile[2][1]][thisFile[2][2]]
                
                love.graphics.setColor(SCMan.colors["GridBorder"])
                love.graphics.draw(thisImg[1], x, y, 0, 1/thisImg[1]:getWidth(), 1/thisImg[1]:getHeight())
            end
            love.graphics.setColor(SCMan.colors["GridBorder"])
            love.graphics.rectangle("line", x, y, 1, 1)
        end
    end

    love.graphics.circle("fill", spawn[1], spawn[2], 0.3)

    love.graphics.setColor(1, 1, 1, 1)
    love.graphics.line(spawn[1], spawn[2], spawn[1]+spawnLook[1], spawn[2]+spawnLook[2])
    love.graphics.setColor(0, 0, 0, 1)
    love.graphics.line(spawn[1]+spawnLook[1]/3, spawn[2]+spawnLook[2]/3, spawn[1]+spawnLook[1]/3*2, spawn[2]+spawnLook[2]/3*2)

    love.graphics.setColor(1, 1, 1, 1)
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

    love.graphics.print("File name: "..fileName, 5, h-85)
    if gridActive then
        love.graphics.print("Grid: active", 5, h-65)
    else
        love.graphics.print("Grid: not active", 5, h-65)
    end
    
    for i = 1,3 do
        love.graphics.setColor(0.6, 0.6, 0.6, 0.4)
        if gridLayer == i then
            love.graphics.setColor(0.8, 0.8, 0.8, 0.8)
        end
        love.graphics.rectangle("fill", w-100, h-60-20*(i-1), 80, 40)
    end

    love.graphics.setColor(1, 1, 1, 1)
    local px, py = getMouseWorldPos()
    local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
    love.graphics.print(px .. " | " .. py, 5, h-45)
    love.graphics.print(pointX .. " | " .. pointY, 5, h-25)

    SCMan.draw();
end

function getMouseWorldPos()
    local mX, mY = love.mouse.getPosition()

    return transform:inverseTransformPoint(mX, mY)
end

function love.keypressed(key)
    if (SCMan.curKeybind ~= nil or SCMan.writingTo ~= nil) then
        SCMan.keypressed(key);
    else
        SCMan.keypressed(key);

        if key == "escape" then
            directoryName = ""
            selected = {"", ""}
            multiSelect = {}
        elseif key == "up" then
            gridLayer = math.min(gridLayer+1, 3)
        elseif key == "down" then
            gridLayer = math.max(gridLayer-1, 1)
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
    if b == 1 then
        if my ~= -2 then
            gridOffsetX, gridOffsetY = gridOffsetX+(x-mx)/scale, gridOffsetY+(y-my)/scale
            if mx-x == 0 and my-y == 0 and selected[1] ~= "" then
                local px, py = getMouseWorldPos()
                local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
                if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
                    placeAt(pointX, pointY)
                end
            end
        end
        mx, my = -1, -1
    elseif b == 2 then
        local px, py = getMouseWorldPos()

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
    scale = math.max(scale + (scale^0.5)*y, 1)
    
end

function love.update(dt)
    globalSpriteIndexHelper = globalSpriteIndexHelper + dt;
    local grid = grid[gridLayer]
    local x, y = love.mouse.getPosition()
    if love.keyboard.isDown("space") and love.mouse.isDown(1) then
        local px, py = getMouseWorldPos()
        local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
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
        end
    else
        grid[gridLayer][x][y][2] = {"", ""}
    end
end

function math.dist(x1,y1, x2,y2) return ((x2-x1)^2+(y2-y1)^2)^0.5 end

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

function love.quit()
    SCMan.savePref()
end