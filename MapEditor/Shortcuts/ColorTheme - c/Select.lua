local json = require "dkjson"
local s = require "ShortcutMultiChoice"

local MyKey = s:new()

MyKey.key = "s"

MyKey.dicToPass = {}

local selected = "ugly"
local colors = {
    ["Default"] = {
        ["BackgroundColor"] = {1, 0, 1},
        ["TextColor"] = {0, 1, 0},
        ["FolderColor"] = {1, 1, 0},
        ["KeybindColor"] = {0, 1, 0.9},
        ["GhostTextColor"] = {0.2, 0.16, 0.04},
        -- ["BackgroundColor"] = {0.2, 0.2, 0.2},
        -- ["TextColor"] = {1, 1, 1},
        -- ["FolderColor"] = {0.6, 0.8, 0.9},
        -- ["KeybindColor"] = {0.7, 0.2, 0.7},
        -- ["GhostTextColor"] = {0.6, 0.6, 0.6},
    },
    ["ugly"] = {
        ["BackgroundColor"] = {1, 0, 1},
        ["TextColor"] = {0, 1, 0},
        ["FolderColor"] = {1, 1, 0},
        ["KeybindColor"] = {0, 1, 0.9},
        ["GhostTextColor"] = {0.2, 0.16, 0.04},
    }

}

function MyKey:onActivate()
    self:genDic()
    self:overrideDic(self.dicToPass, self.handler)
end

function MyKey:onGetResult(obj)
    selected = obj[2]
    
    self.handler.colors = colors[selected]
end

function MyKey:genDic()
    self.dicToPass = {}
    
    local i = 1
    for key, color in pairs(colors) do
        table.insert(self.dicToPass, {tostring(i), key})
        i = i + 1
    end
end

function MyKey:loadString(v)
    local decoded = json.decode(v)
    colors = decoded[1]
    selected = decoded[2]

    self.handler.colors = colors[selected]
end

function MyKey:saveString()
    return json.encode({colors, selected})
end

function MyKey:getRelevant()
    return colors, selected
end

return MyKey
