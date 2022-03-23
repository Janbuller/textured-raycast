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
