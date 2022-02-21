function loadImage(path)
    local linesFromFile = {}
    for line in love.filesystem.lines(path) do table.insert(linesFromFile, line) end
    if (linesFromFile[1] ~= "P3") then
        return
    end
    local lines = {}
    for _, line in pairs(linesFromFile) do
        if string.sub(line, 0, 1) ~= "#" then
            table.insert(lines, line)
        end
    end
    local nrs = string.numsplit(lines[2], " ")
    local w, h = nrs[1], nrs[2]
    local maxColVal = tonumber(lines[3])
    local pixles = {}

    for i = 4,#lines, 3 do
        local line = lines[i]
        local color = {0, 0, 0}
        for j = 0,2 do
            color[j+1] = tonumber(lines[i+j])/255
        end
        pixles[(i-4)/3+1] = color
    end

    local thisP = 1
    local iData = love.image.newImageData(h, w)
    for x = 0,w-1 do
        for y = 0,h-1 do
            iData:setPixel(y, x, pixles[thisP][1], pixles[thisP][2], pixles[thisP][3], 1)
            thisP = thisP + 1
        end
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
    "img/skybox.ppm",
    "img/wolfenstein/end.ppm",
    "img/wolfenstein/exit.ppm",
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

function newGrid(gW, gH)
    local grid = {}

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
            love.graphics.setColor(0, 0, 0)
            love.graphics.rectangle("line", x, y, 1, 1)
        end
    end

    love.graphics.setColor(1, 1, 1)
    if openMen then
        love.graphics.rectangle("fill", px-gW/2+1, py-gH/2+1, 0.5, 0.8)
    end

    if drawSelect then
        love.graphics.origin()
        love.graphics.setLineWidth(2)
        
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

function love.keypressed(key)
    if key == "m" then
        drawSelect = not drawSelect
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
        px, py = ((x-w/2-gridOffsetX)/scale)+gW/2-1, ((y-h/2-gridOffsetY)/scale)+gH/2-1
        selectedForMenuX, selectedForMenuY = math.floor((x-w/2-gridOffsetX)/scale)+gW/2, math.floor((y-h/2-gridOffsetY)/scale)+gH/2
        openMen = true
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