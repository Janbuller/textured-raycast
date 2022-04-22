using System.Collections.Generic;
using textured_raycast.maze.Items;
using textured_raycast.maze.skills;

namespace textured_raycast.maze
{
    enum EquipSlots
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

        public float money = 0;

        public float actualHp = 10;

        public Dictionary<EquipSlots, Item> equipped = new Dictionary<EquipSlots, Item>()
        {
            {EquipSlots.Head, null},
            {EquipSlots.Accessory1, null},
            {EquipSlots.Accessory2, null},
            {EquipSlots.MainHand, null},
            {EquipSlots.OffHand, null},
            {EquipSlots.Torso, null},
        };

        public Dictionary<int, EquipSlots> guiToEquipped = new Dictionary<int, EquipSlots>()
        {
            {2, EquipSlots.Head},
            {8, EquipSlots.Accessory1},
            {9, EquipSlots.Accessory2},
            {4, EquipSlots.MainHand},
            {6, EquipSlots.OffHand},
            {5, EquipSlots.Torso},
        };

        public void reset()
        {
            actualHp = hp;
            xp = 0;
        }

        public void useSkill(int nr)
        {
            if (equippedSkills[nr] != -1)
            {
                if (Skill.Skills[equippedSkills[nr]] is IActiveSkill)
                {
                    (Skill.Skills[equippedSkills[nr]] as IActiveSkill).Activate();

                    if (World.fight.hp <= 0)
                        World.fight.enemyDead();

                    World.fight.enemyDoAction();
                }
            }
        }

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
            { 0,  new Iron_Broadsword()},
            { 1,  new Ruby_Ring()},
            { 2,  new Iron_Armor()},
            { 3,  new Iron_Helmet()},
            { 4,  new Stick()},
            { 5,  new Stone_Sword()},
            { 6,  new Training_Sword()},
            { 7,  new Wooden_Axe()},
            { 8,  new Wooden_Battleaxe()},
            { 9,  new Wooden_Sword()},
            { 10, new Iron_Knife()},
            { 11, new Small_Iron_Sword()},
            { 12, new Iron_Dagger()},
            { 13, new Iron_Spear()},
            { 14, new Iron_Axe()},
            { 15, new Iron_Battleaxe()},
            { 16, new Iron_Scythe()},
            { 17, new Flint_Spear()},
            { 18, new Flint_Axe()},
            { 19, new Flint_Scythe()},
            { 20, new Adamantite_Saber()},
            { 21, new Adamantite_Seax()},
            { 22, new Adamantite_Axe()},
            { 23, new Adamantite_Labrys()},
            { 24, new Adamantite_Scythe()},
            { 25, new Palladium_Broadsword()},
            { 26, new Palladium_Slendersword()},
            { 27, new Palladium_Axe()},
            { 28, new Palladium_Labrys()},
            { 29, new Palladium_Scythe()},

            { 30, new Mithril_Broadsword()},
            { 31, new Mithril_Longsword()},
            { 32, new Mithril_Axe()},
            { 33, new Mithril_Battleaxe()},
            { 34, new Radium_Sword()},
            { 35, new Radium_Axe()},
            { 36, new Radium_Battleaxe()},
            { 37, new Cobalt_Battleaxe()},
            { 38, new Cobalt_Sword()},
            { 39, new Luminite_Blade()},
            { 40, new Jade_Cutlass()},
            { 41, new Amethyst_Falcion()},
            { 42, new Bismuth_Claymore()},

            { 43, new Apple()},
            { 44, new Banana()},
            { 45, new Buger()},
            { 46, new Cherries()},
            { 47, new Kiwi()},
            { 48, new Orange()},
            { 49, new Pear()},
            { 50, new Strawberry()},
            // { 4, new Iron_Broadsword()},
            // { 7, new Iron_Helmet()},
            // { 6, new Iron_Broadsword()},
            // { 26, new Iron_Broadsword()},
            // { 49, new Iron_Broadsword()},
            // { 50, new Iron_Broadsword()},
            // { 62, new Iron_Broadsword()},
            // { 2, new Ruby_Ring()},
            // { 3, new Ruby_Ring()},
            // { 5, new Ruby_Ring()},
            // { 10, new Iron_Armor()},
            // { 11, new Iron_Armor()},
            // { 12, new Iron_Armor()},
        };

        public int lvl = 1;
        public float xp = 0;
        public int skillPoints = 0;
        public List<int> UnlockedSkills = new List<int>() { 12 };

        public int[] equippedSkills =
        {
            12,
            -1,
            -1,
        };
    }
}
