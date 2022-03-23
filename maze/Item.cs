using System.Collections.Generic;

namespace textured_raycast.maze
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
