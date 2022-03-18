using rpg_game.maze;
using rpg_game.maze.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace textured_raycast.maze
{
    enum equipSlots
    {
        Head,
        Accessory1,
        Accessory2,
        MainHand,
        OffHand,
        Torso,
    }

    internal class Player
    {
        public int dam = 2;
        public int hp = 10;
        public int mag = 2;

        public Dictionary<equipSlots, Item> equipped = new Dictionary<equipSlots, Item>()
        {
            {equipSlots.Head, null},
            {equipSlots.Accessory1, null},
            {equipSlots.Accessory2, null},
            {equipSlots.MainHand, null},
            {equipSlots.OffHand, null},
            {equipSlots.Torso, null},
        };

        public Dictionary<int, equipSlots> guiToEquipped = new Dictionary<int, equipSlots>()
        {
            {2, equipSlots.Head},
            {8, equipSlots.Accessory1},
            {9, equipSlots.Accessory2},
            {4, equipSlots.MainHand},
            {6, equipSlots.OffHand},
            {5, equipSlots.Torso},
        };

        public void addToInv(Item item)
        {
            bool placed = false;
            int i = 0;
            while (placed == false)
            {
                if (!inv.ContainsKey(i))
                {
                    placed = true;
                    inv.Add(i, item);
                    return;
                }
                i++;
            }
        }

        public int invSelectedSpot = -1;

        public Dictionary<int, Item> inv = new Dictionary<int, Item>()
        {
            { 4, new Iron_Broadsword()},
            { 6, new Iron_Broadsword()},
            { 26, new Iron_Broadsword()},
            { 49, new Iron_Broadsword()},
            { 50, new Iron_Broadsword()},
            { 62, new Iron_Broadsword()},
            { 2, new Ruby_Ring()},
            { 3, new Ruby_Ring()},
            { 5, new Ruby_Ring()},
            { 10, new Iron_Armor()},
            { 11, new Iron_Armor()},
            { 12, new Iron_Armor()},
        };
        int skillPoints = 25;
    }
}
