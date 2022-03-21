using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;

namespace rpg_game.maze
{
    abstract class Item
    {
        public static Dictionary<int, string> itemTextures = new Dictionary<int, string>() {
            {0,   "img/items/metalsword.ppm"},
            {1,   "img/items/rubyring.ppm"},
            {2,   "img/items/metalarmor.ppm"},
        };


        public string name = "";
        public int imageID = 0;

        public Dictionary<equipSlots, bool> tags = new Dictionary<equipSlots, bool>()
        {
            {equipSlots.Head, false},
            {equipSlots.Accessory1, false},
            {equipSlots.Accessory2, false},
            {equipSlots.MainHand, false},
            {equipSlots.OffHand, false},
            {equipSlots.Torso, false},
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
