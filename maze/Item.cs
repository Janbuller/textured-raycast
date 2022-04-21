using System.Collections.Generic;

namespace textured_raycast.maze
{
    abstract class Item
    {
        public static Dictionary<int, string> itemTextures = new Dictionary<int, string>() {
            {0,   "img/items/metalsword.ppm"},
            {1,   "img/items/rubyring.ppm"},
            {2,   "img/items/metalarmor.ppm"},
            {3,   "img/items/metalhelmet.ppm"},
            {4,   "img/items/stick.ppm"},
            {5,   "img/items/stonesword.ppm"},
            {6,   "img/items/trainingsword.ppm"},
            {7,   "img/items/woodaxe.ppm"},
            {8,   "img/items/woodbattleaxe.ppm"},
            {9,   "img/items/woodensword.ppm"},
            {10,  "img/items/metalknife.ppm"},
            {11,  "img/items/metalswordsmall.ppm"},
            {12,  "img/items/metaldagger.ppm"},
            {13,  "img/items/metalspear.ppm"},
            {14,  "img/items/metalaxe.ppm"},
            {15,  "img/items/metalbattleaxe.ppm"},
            {16,  "img/items/metalscythe.ppm"},
            {17,  "img/items/flintspear.ppm"},
            {18,  "img/items/flintaxe.ppm"},
            {19,  "img/items/flintscythe.ppm"},
            {20,  "img/items/adamantitesword.ppm"},
            {21,  "img/items/adamantiteswordsmall.ppm"},
            {22,  "img/items/adamantiteaxe.ppm"},
            {23,  "img/items/adamantitebattleaxe.ppm"},
            {24,  "img/items/adamantitescythe.ppm"},
            {25,  "img/items/palladiumsword.ppm"},
            {26,  "img/items/palladiumswordsmall.ppm"},
            {27,  "img/items/palladiumaxe.ppm"},
            {28,  "img/items/palladiumbattleaxe.ppm"},
            {29,  "img/items/palladiumscythe.ppm"},
            {30,  "img/items/mithrilsword.ppm"},
            {31,  "img/items/mithrilswordsmall.ppm"},
            {32,  "img/items/mithrilaxe.ppm"},
            {33,  "img/items/mithrilbattleaxe.ppm"},
            {34,  "img/items/radiumsword.ppm"},
            {35,  "img/items/radiumaxe.ppm"},
            {36,  "img/items/radiumbattleaxe.ppm"},
            {37,  "img/items/cobaltbattleaxe.ppm"},
            {38,  "img/items/cobaltsword.ppm"},
            {39,  "img/items/luminitesword.ppm"},
            {40,  "img/items/jadesword.ppm"},
            {41,  "img/items/amethystsword.ppm"},
            {42,  "img/items/bismuthsword.ppm"},
        };


        public string name = "";
        public int imageID = 0;

        public Dictionary<EquipSlots, bool> tags = new Dictionary<EquipSlots, bool>()
        {
            {EquipSlots.Head, false},
            {EquipSlots.Accessory1, false},
            {EquipSlots.Accessory2, false},
            {EquipSlots.MainHand, false},
            {EquipSlots.OffHand, false},
            {EquipSlots.Torso, false},
        };

        public int addHP = 0;
        public int addDAM = 0;
        public int addMAG = 0;

        public void onEquip()
        {
            World.player.hp += addHP;
            World.player.dam += addDAM;
            World.player.mag += addMAG;
        }

        public void onUnEquip()
        {
            World.player.hp -= addHP;
            World.player.dam -= addDAM;
            World.player.mag -= addMAG;
        }
    }
}
