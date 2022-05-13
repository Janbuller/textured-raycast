local s = require "Shortcut"

local MyKey = s:new()

MyKey.key = "s"

function MyKey:onActivate()
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

function getPath()
    local sys = love.system.getOS()
    if sys == "Windows" then
        return "../game/bin/Debug/netcoreapp3.1/maps/"
    elseif sys == "Linux" then
        return "../game/maps/"
    end
end

function pathListToString(list) -- for sprites
   local str = ""
   for _, path in pairs(list) do
       str = str..folders[path[1]][path[2]][2].."-"
   end
   str = string.sub(str, 0, #str-1)
   return str
end

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

function MyKey:onReciveText(text)
end

return MyKey
