-- load the shortcut handeler, a shortcut module im verry proud of, and will use a lot in the future
local SCMan = require("ShortcutHandler")

-- make pixle art not have blur between pixles
love.graphics.setDefaultFilter("nearest", "nearest")

-- laod a ppm as an iamge
function loadImage(path)
    -- load the path of the image
    local str, len = love.filesystem.read(path)

    -- remove all comments
    str = string.gsub(str, "#[^\r\n]+\r?\n", "")

    -- get widht height and max color, (fx 1 makes colors range form 0 to 1 and 255 dose the same for 255)
    local _,start, w, h, colorMax = string.find(str, "(%d+) (%d+)\r?\n(%d+)")
    -- make a new image data based on the widht and height
    local iData = love.image.newImageData(tonumber(w), tonumber(h))
    -- make the color max a number
    colorMax = tonumber(colorMax)

    -- make index
    local i = 0
    -- make list to save color
    local thisColor = {}
    for strPart in string.gmatch(str, "(%d+)") do -- regex numbers
        if i > 3 then -- skip first 3 numbers cuz w, h and color
            -- get if the color is red green or blue
            local colorPos = ((i-1)%3)+1
            -- place it in the color list
            thisColor[colorPos] = tonumber(strPart)/colorMax
            -- if it is the last color (blue) add it to the image data
            if i%3 == 0 then
                -- get the position
                local pos = math.ceil(i/3)-2
                -- a for alpha
                local a = 1
                if thisColor[1]==0 and thisColor[2]==0 and thisColor[3]==0 then
                    a = 0 -- if all colors are 0, then set alpha to 0
                end
                -- set the image data at the position
                iData:setPixel(pos-math.floor(pos/w)*w, math.floor(pos/w), thisColor[1], thisColor[2], thisColor[3], a)
            end
        end
        -- add index
        i = i + 1
    end

    --return image data as image
    return love.graphics.newImage(iData)
end

-- for animations
globalSpriteIndexHelper = 1;

-- takes a string and split it to numbers with delimiter as the sepperator
function string.numsplit(s, delimiter)
    local result = {}
    for match in (s..delimiter):gmatch("(.-)"..delimiter) do
        table.insert(result, tonumber(match))
    end
    return result
end

-- same as abouve but returns as normal strings
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

-- go thrugh all files in img
for _, fileName in pairs(namesOfFiles) do
    -- check if file is a directory (cuz we only going 1 layer deep)
    if love.filesystem.getInfo("img/"..fileName).type == "directory" then
        -- get the list of files for the folder/directory
        local namesOfFiles2 = love.filesystem.getDirectoryItems("img/"..fileName)
        -- make a new directory item in folders, that is a list
        folders[fileName] = {}
        for _, fileName2 in pairs(namesOfFiles2) do
            -- check if the file from the folder is a .ppm file
            if string.sub(fileName2, #fileName2-3, #fileName2) == ".ppm" then
                -- load it in the dictionary as a new dictionart index, with loadimage, and save the path
                folders[fileName][fileName2] = {loadImage("img/"..fileName.."/"..fileName2), "img/"..fileName.."/"..fileName2, {fileName, fileName2}}
            end
        end
    end
end

-- make a new transform (a thing that can offset everything on scren based on certian parameters)
transform = love.math.newTransform()

-- declare variables
w, h = love.graphics.getWidth(), love.graphics.getHeight() -- screen w and h
grid = {}
gW, gH = 20, 20 -- grid w and h
gridLayer = 2 -- what layer we are on
gridOffsetX, gridOffsetY = 0, 0 -- offset for mving screen
mx, my = -1, -1 -- mouse position for moving screne
scale = 20 -- scale for zooming

selected = {"", ""} -- the path of the selected image, if any

guiTileSize = 40 -- size of image itles
guiTilediff = 6 -- width between images
guiMaxTiles = 17 -- max tiles on width (for wrapping)

gridActive = false -- if grid is active (grid snapping)

spawn = {0, 0} -- player spawn
spawnLook = {0, 0} -- player spawn rotation

directoryName = "" -- name of currentyl opend dirrectory

sprites = {} -- list of all sprites

fileName = "newMap" -- name to save map as

function newGrid(gWin, gHin) -- clear grid / fill it with black images
    local grid = {}
    gW, gH = gWin, gHin
    for i = 1,3 do -- for each layer (floor wall and roof)
        grid[i] = {}
        for x = 1,gW do -- for x
            grid[i][x] = {}
            for y = 1,gH do -- for y
                grid[i][x][y] = {"", {"", ""}} -- empty path, no image
            end
        end
    end
    
    return grid
end

multiSelect = {} -- list if all seleced images (for animations)

grid = newGrid(20, 20) -- make a new grid 20x20 default size

function love.load()
    SCMan.load(); -- laod short cut manager
end

function love.textinput(t) -- setup texinput with short cut manager
   SCMan.textinput(t);
end

function love.draw()
    love.graphics.setBackgroundColor(SCMan.colors["AllBackground"])
    -- layer of the grid
    local grid = grid[gridLayer] -- only focus on current grid layer

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
            -- get the image path
            local thisFile = grid[x+gW/2][y+gH/2]
            if thisFile[2][1] == "" then -- if its empty draw a rectangle with gridbackground color form short cut manager
                love.graphics.setColor(SCMan.colors["GridBackground"])
                love.graphics.rectangle("fill", x, y, 1, 1)
            elseif thisFile[2][1] ~= "" then -- if not, get the image
                local thisImg = folders[thisFile[2][1]][thisFile[2][2]]
                
                -- draw the image, set color to white(cuz 1 red 1 green and 0 blue, will draw the image wiht no blue)
                love.graphics.setColor(1, 1, 1)
                love.graphics.draw(thisImg[1], x, y, 0, 1/thisImg[1]:getWidth(), 1/thisImg[1]:getHeight())
            end

            -- draw the border lines
            love.graphics.setColor(SCMan.colors["GridBorder"])
            love.graphics.rectangle("line", x, y, 1, 1)
        end
    end

    -- draw player at "spawn"
    love.graphics.circle("fill", spawn[1], spawn[2], 0.3)

    -- draw a line that represents player rotation
    love.graphics.setColor(0, 0, 0, 1)
    love.graphics.line(spawn[1]+spawnLook[1]/3, spawn[2]+spawnLook[2]/3, spawn[1]+spawnLook[1]/3*2, spawn[2]+spawnLook[2]/3*2)

    -- draw all sprites, set color to white
    love.graphics.setColor(1, 1, 1, 1)
    for _, sprite in pairs(sprites) do
        local roundDown = math.floor(globalSpriteIndexHelper) -- get rotation index round down
        local maxSprites = #sprite[3] -- get amount of frames in sprites
        local thisIndex = roundDown%maxSprites+1 -- get the current animation frame based on the 2 other variables
        
        -- gets the image of the sprite
        local thisImg = folders[sprite[3][thisIndex][1]][sprite[3][thisIndex][2]]

        -- draw it, but scaled down to 0.6, 0.6 w and h
        love.graphics.draw(thisImg[1], sprite[1]-0.3, sprite[2]-0.3, 0, 0.6/thisImg[1]:getWidth(), 0.6/thisImg[1]:getHeight())
    end
    
    -- return transform to origin, for gui
    love.graphics.origin()
    love.graphics.setLineWidth(2)

    -- if the direcroty is == "" then the player hasent chose a folder, so show folders
    if directoryName == "" then
        local i = 0 -- make a counter for folders, i
        for _, folderName in pairs(folders) do
            i = i + 1 -- add to counter

            -- draw the folder with ImageFolderColro form SCMan, based on i and a lot of maths
            love.graphics.setColor(SCMan.colors["ImageFolderColor"])
            love.graphics.rectangle("fill", 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1, 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1, (guiTileSize+guiTilediff/2),  (guiTileSize+guiTilediff/2))
            
            -- draw the text at the same position but with text color and... text
            love.graphics.setColor(SCMan.colors["ImageFolderText"])
            love.graphics.printf(_, 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles), 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1), (guiTileSize+guiTilediff/2), "center")
        end
    else -- if a folder is cosen draw its contents
        local i = 0 -- same with counter
        for _, imageNPath in pairs(folders[directoryName]) do
            i = i + 1 -- add to counter

            -- white for image background
            love.graphics.setColor(1, 1, 1)

            -- go thru all selected images, and if the current image to draw
            -- is in the images selected, change color of the image background yellow
            -- and act like some sort of hover thing
            for _, selected in pairs(multiSelect) do
                if selected[1] == imageNPath[3][1] and selected[2] == imageNPath[3][2] then
                    love.graphics.setColor(1, 1, 0)
                end
            end

            -- draw backround rectangle
            love.graphics.rectangle("fill", 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1, 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1, (guiTileSize+guiTilediff/2),  (guiTileSize+guiTilediff/2))
            
            -- set to white, for draw, and draw
            love.graphics.setColor(1, 1, 1)
            love.graphics.draw(imageNPath[1], 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles), 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1), 0, guiTileSize/imageNPath[1]:getWidth(), guiTileSize/imageNPath[1]:getHeight())
        end
    end


    -- white for white text
    love.graphics.setColor(1, 1, 1)

    -- write filename to screen
    love.graphics.print("File name: "..fileName, 5, h-85)
    -- write if grid is active or not
    if gridActive then
        love.graphics.print("Grid: active", 5, h-65)
    else
        love.graphics.print("Grid: not active", 5, h-65)
    end
    
    -- go thru all layers and draw them, basically 3 boxes with transparrency
    for i = 1,3 do
        love.graphics.setColor(0.6, 0.6, 0.6, 0.4)
        if gridLayer == i then
            love.graphics.setColor(0.8, 0.8, 0.8, 0.8)
        end
        love.graphics.rectangle("fill", w-100, h-60-20*(i-1), 80, 40)
    end

    -- set color to white
    love.graphics.setColor(1, 1, 1, 1)
    local px, py = getMouseWorldPos()
    local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
    -- get world mouse x, y and world mouse grid x and y (what tile is getting hovered)
    love.graphics.print(px .. " | " .. py, 5, h-45)
    love.graphics.print(pointX .. " | " .. pointY, 5, h-25)

    -- draw shortcur manager, if theres anything to draw
    SCMan.draw();
end

function getMouseWorldPos()
    local mX, mY = love.mouse.getPosition()
    -- get screen position and use transform to do inverse transform and get world position
    return transform:inverseTransformPoint(mX, mY)
end

function love.keypressed(key) -- if a key is pressd
    -- if SCMan is currently getting any sort of input
    -- then only send values to scman, else check the other
    -- things as well
    if (SCMan.curKeybind ~= nil or SCMan.writingTo ~= nil) then
        SCMan.keypressed(key);
    else
        SCMan.keypressed(key);

        -- if you press escape, go back in dictionary
        if key == "escape" then
            directoryName = ""
            selected = {"", ""}
            multiSelect = {}
        elseif key == "up" then -- if key up is pressed, go a layer up
            gridLayer = math.min(gridLayer+1, 3)
        elseif key == "down" then -- down is a layer down
            gridLayer = math.max(gridLayer-1, 1)
        end
    end
end

function love.mousepressed(x, y, b)
    -- if button(b) is 1/leftclick
    -- and y is in the top of the screen (within the gui area for images)
    if b == 1 and y < 2+(guiTileSize+guiTilediff)*(math.ceil(getLenOfCurImage()/guiMaxTiles)) then
        -- get amount of images (or dictionarys)
        for i = 1, getLenOfCurImage() do
            -- if x and y is within the current folder position... (a crapton of maths)
            if x > 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 and x < 2+(guiTileSize+guiTilediff)*(i-1)-((math.ceil(i/guiMaxTiles)-1)*(guiTileSize+guiTilediff)*guiMaxTiles)-1 + (guiTileSize+guiTilediff/2) then
                if y > 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 and y < 2+(guiTileSize+guiTilediff)*(math.ceil(i/guiMaxTiles)-1)-1 + (guiTileSize+guiTilediff/2) then
                    -- if we are browsering dictionaries
                    if directoryName == "" then
                        local i2 = 0;
                        -- go through all dictionaries
                        for folderName, _ in pairs(folders) do
                            i2 = i2 + 1
                            if i2 == i then
                                -- and find the dictionary with the same index as the box we should be within right now
                                -- thanks to the 2 if's
                                directoryName = folderName
                            end
                        end
                    else
                        -- if it is not a folder
                        local i2 = 0;
                        for dirName, _  in pairs(folders[directoryName]) do
                            i2 = i2 + 1
                            -- go through all images in current folder
                            if i2 == i then
                                -- if the index is the same
                                if love.keyboard.isDown("lshift") then
                                    -- add to mulri select if lshift is pressed
                                    table.insert(multiSelect, {directoryName, dirName})
                                else
                                    -- if lshift is not pressed then clear multiselect
                                    -- then add this to it, and set select to this as well
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
        my = -2 -- set my to -2 so that it dosent thing that this is a mouse drag
    elseif b == 3 then -- if button 3 is pressed (middle mouse button)
        local px, py = getMouseWorldPos()
        local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
        -- get mouse position, but its grid positon

        if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
            -- then set selected to the grid we found, if it is within grid...
            directoryName = grid[gridLayer][pointX][pointY][2][1]
            selected = grid[gridLayer][pointX][pointY][2]
        end
    else
        -- if y is not over that threshold from beffore and space is not down
        if b == 1 and not love.keyboard.isDown("space") then
            mx, my = x, y -- start moving
        elseif love.keyboard.isDown("space") then
            my = -2 -- make it not move
        end
    end
end

function love.mousereleased(x, y, b)
    local grid = grid[gridLayer]
    -- get the current grid
    if b == 1 then
        -- if you release leftclick
        if my ~= -2 then
            -- and my dosent equals -2, so you need to move
            --recalculate grudoffset x and y
            gridOffsetX, gridOffsetY = gridOffsetX+(x-mx)/scale, gridOffsetY+(y-my)/scale

            -- make sure no funny busness has been done those equals 0 things
            if mx-x == 0 and my-y == 0 and selected[1] ~= "" then
                local px, py = getMouseWorldPos()
                local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2

                -- if grid position x and y is within the grid
                if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
                    placeAt(pointX, pointY) -- run function...
                end
            end
        end
        mx, my = -1, -1 -- reset mx and my (screen drag)
    elseif b == 2 then -- if button was right clcik
        local px, py = getMouseWorldPos()

        -- get position
        if selected[1] ~= "" then
            -- add a sprite, and if grid was active, then 
            if gridActive then
                table.insert(sprites, {math.ceil((px-0.25)*2)/2, math.ceil((py-0.25)*2)/2, {}, ""})
            else
                table.insert(sprites, {px, py, {}, ""})
            end

            -- place all multi selected images, into the sprite, for animations sake
            for i, selected in ipairs(multiSelect) do
                sprites[#sprites][3][i] = selected
            end
        end
    end
end

function love.wheelmoved(x, y) -- if you move mousewheel
    local mx, my = love.mouse.getPosition()

    -- change scale
    scale = math.max(scale + (scale^0.5)*y, 1)
end

function love.update(dt) -- run every frame, returns delta time (time between frames)
    globalSpriteIndexHelper = globalSpriteIndexHelper + dt;
    -- add dt to sprite index, for animations

    local grid = grid[gridLayer]
    -- get grid for cur layer

    local x, y = love.mouse.getPosition()
    -- get x and y pos

    -- if left is held and space is held
    if love.keyboard.isDown("space") and love.mouse.isDown(1) then
        local px, py = getMouseWorldPos()
        local pointX, pointY = math.floor(px)+gW/2, math.floor(py)+gH/2
        if pointX > 0 and pointX < gW+1 and pointY > 0 and pointY < gH+1 then
            placeAt(pointX, pointY) -- try to draw, like beffore
        end
    end
end

function placeAt(x, y)
    if selected[1] ~= "" then -- check if image is selected
        -- if you have a image selected then
        if love.keyboard.isDown("lshift") then
            -- if lshift is down, erase
            grid[gridLayer][x][y][2] = {"", ""}
        else
            -- else draw the currently selected image
            grid[gridLayer][x][y][2] = selected
        end
    else
        -- if no image is selected, erase
        grid[gridLayer][x][y][2] = {"", ""}
    end
end

-- pytagoras distance
function math.dist(x1,y1, x2,y2) return ((x2-x1)^2+(y2-y1)^2)^0.5 end

function getLenOfCurImage()
    local len = 0

    -- get lenght of the current image related thing
    if directoryName == "" then
        -- if no directory is selected, get amount of folders
        len = lenOfPAIRSList(folders)
    else
        -- same but for images, if folder is selected
        len = lenOfPAIRSList(folders[directoryName])
    end

    return len
end

function lenOfPAIRSList(list)
    local len = 0
    for i, v in pairs(list) do
        -- go thrugh all things in list
        len = len + 1
        -- and cound them
    end
    return len
end

function love.quit()
    -- when quitting save Shortcutmanager prefferences
    SCMan.savePref()
end