using System;
using System.Collections.Generic;
using textured_raycast.maze.resources;
using textured_raycast.maze.texture;

namespace textured_raycast.maze
{
    abstract class Item
    {
        // all items by id
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
            {43,  "img/items/food_apple.ppm"},
            {44,  "img/items/food_banana.ppm"},
            {45,  "img/items/food_burger.ppm"},
            {46,  "img/items/food_cherries.ppm"},
            {47,  "img/items/food_kiwi.ppm"},
            {48,  "img/items/food_orange.ppm"},
            {49,  "img/items/food_pear.ppm"},
            {50,  "img/items/food_strawberry.ppm"},

            {51,  "img/items/woodshield.ppm"},
            {52,  "img/items/woodshieldinset.ppm"},
            {53,  "img/items/shield1.ppm"},
            {54,  "img/items/shield2.ppm"},
            {55,  "img/items/woodhelmet.ppm"},
            {56,  "img/items/woodring.ppm"},
            {57,  "img/items/metalring.ppm"},
            {58,  "img/items/goldring.ppm"},
            {59,  "img/items/crossnecklace.ppm"},
            {60,  "img/items/cross.ppm"},
            {61,  "img/items/torch.ppm"},
        };


        public string name = "";
        public int imageID = 0; // the id of the image to use form above

        // the places this item can be equipped
        public Dictionary<EquipSlots, bool> tags = new Dictionary<EquipSlots, bool>()
        {
            {EquipSlots.Head, false},
            {EquipSlots.Accessory1, false},
            {EquipSlots.Accessory2, false},
            {EquipSlots.MainHand, false},
            {EquipSlots.OffHand, false},
            {EquipSlots.Torso, false},
        };

        // the amount of hp, mg, dam and give-hp(for healing) to give the player on equip / consume
        public int addHP = 0;
        public int addDAM = 0;
        public int addMAG = 0;
        public int giveHP = 0;

        // if the item emitting light
	    public bool light = false;

        // eat an item
        public virtual bool consume()
        {
            if (giveHP > 0)
            {
                World.player.actualHp = Math.Min(World.player.Hp, World.player.actualHp + giveHP);
                return true;
            }
            return false;
        }

        // give stats on equip
        public virtual void onEquip()
        {
            World.player.hp += addHP;
            World.player.actualHp = Math.Min(Math.Max(World.player.actualHp + addHP, 1), World.player.Hp);
            World.player.dam += addDAM;
            World.player.mag += addMAG;
        }

        // take the away on unequip
        public virtual void onUnEquip()
        {
            World.player.hp -= addHP;
            World.player.actualHp = Math.Min(Math.Max(World.player.actualHp - addHP, 1), World.player.Hp);
            World.player.dam -= addDAM;
            World.player.mag -= addMAG;
        }

        public virtual Texture getTexture()
        {
            // return the texture of the item
            return ResourceManager.getTexture(itemTextures[imageID]);
        }
    }
}
