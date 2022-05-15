using System;
using System.Collections.Generic;
using textured_raycast.maze.Items;
using textured_raycast.maze.skills;
using textured_raycast.maze.input;
using textured_raycast.maze.skills.Skills;

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
        // internal stats
        public int dam = 2;
        public int hp = 10;
        public int mag = 2;

        // external stats, with potential buffs applied
        public int Dam
        {
            get { return dam * (100 + addPPhys) / 100; } // apply dam %
            set { dam = value; }
        }
        public int Hp
        {
            get { return hp; }
            set { hp = value; }
        }
        public int Mag
        {
            get { return mag * (100 + addPMag) / 100; } // apply mag %
            set { mag = value; }
        }

        // buffs the player can get
        public int addPLifeSteal = 0;
        public int addPMag = 0;
        public int addPPhys = 0;

        public bool don = false; // double or nothing

        // the adventurere level of the player
        public int adventureLVL = 0;
        //1: Bronze
        //2: Silver
        //3: Gold
        //4: Diamond
        //5: Mythril

        // the players money
        public float money = 0;

        // the active hp, Hp is just the max
        public float actualHp = 10;

        // a dictionary of all slots to that can bear items
        public Dictionary<EquipSlots, Item> equipped = new Dictionary<EquipSlots, Item>()
        {
            {EquipSlots.Head,       null},
            {EquipSlots.Accessory1, null},
            {EquipSlots.Accessory2, null},
            {EquipSlots.MainHand,   null},
            {EquipSlots.OffHand,    null},
            {EquipSlots.Torso,      null},
        };

        // a list to convert from gui box to equipment slot
        public Dictionary<int, EquipSlots> guiToEquipped = new Dictionary<int, EquipSlots>()
        {
            {2, EquipSlots.Head},
            {8, EquipSlots.Accessory1},
            {9, EquipSlots.Accessory2},
            {4, EquipSlots.MainHand},
            {6, EquipSlots.OffHand},
            {5, EquipSlots.Torso},
        };

        // checks if player is holding an item that has light (like a torch, but we have a torch you just cant get it in the game...)
        public bool HoldsLight
        {
            get
            {
                foreach (var item in equipped)
                {
                    if (item.Value is null)
                        continue;
                    if (item.Value.light)
                        return true;
                }
                return false;
            }
        }

        // hp = max hp, and half xp
        public void reset()
        {
            actualHp = Hp;
            xp /= 2;
        }

        // check if the skill with the given index exists, then try to activate it.
        public void useSkill(int nr)
        {
            if (equippedSkills[nr] != -1)
            {
                if (Skill.Skills[equippedSkills[nr]] is IActiveSkill)
                {
                    (Skill.Skills[equippedSkills[nr]] as IActiveSkill).Activate();

                    if (World.fight.hp <= 0)
                        World.fight.enemyDead();
                    else
                        World.fight.enemyDoAction();
                }
            }
        }

        // add an item to the inventory
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

        // for holding track of player 'cursor' in the inventory
        public int invSelectedSpot = -1;

        // list of all items in the inventory, you could uncomment these to try being op... lol
        public Dictionary<int, Item> inv = new Dictionary<int, Item>()
        {
            //{ 0,  new Iron_Broadsword()},
            //{ 1,  new Ruby_Ring()},
            //{ 2,  new Iron_Armor()},
            //{ 3,  new Iron_Helmet()},
            //{ 4,  new Stick()},
            //{ 5,  new Stone_Sword()},
            //{ 6,  new Training_Sword()},
            //{ 7,  new Wooden_Axe()},
            //{ 8,  new Wooden_Battleaxe()},
            //{ 9,  new Wooden_Sword()},
            //{ 10, new Iron_Knife()},
            //{ 11, new Small_Iron_Sword()},
            //{ 12, new Iron_Dagger()},
            //{ 13, new Iron_Spear()},
            //{ 14, new Iron_Axe()},
            //{ 15, new Iron_Battleaxe()},
            //{ 16, new Iron_Scythe()},
            //{ 17, new Flint_Spear()},
            //{ 18, new Flint_Axe()},
            //{ 19, new Flint_Scythe()},
            //{ 20, new Adamantite_Saber()},
            //{ 21, new Adamantite_Seax()},
            //{ 22, new Adamantite_Axe()},
            //{ 23, new Adamantite_Labrys()},
            //{ 24, new Adamantite_Scythe()},
            //{ 25, new Palladium_Broadsword()},
            //{ 26, new Palladium_Slendersword()},
            //{ 27, new Palladium_Axe()},
            //{ 28, new Palladium_Labrys()},
            //{ 29, new Palladium_Scythe()},

            //{ 30, new Mithril_Broadsword()},
            //{ 31, new Mithril_Longsword()},
            //{ 32, new Mithril_Axe()},
            //{ 33, new Mithril_Battleaxe()},
            //{ 34, new Radium_Sword()},
            //{ 35, new Radium_Axe()},
            //{ 36, new Radium_Battleaxe()},
            //{ 37, new Cobalt_Battleaxe()},
            //{ 38, new Cobalt_Sword()},
            //{ 39, new Luminite_Blade()},
            //{ 40, new Jade_Cutlass()},
            //{ 41, new Amethyst_Falcion()},
            //{ 42, new Bismuth_Claymore()},

            //{ 43, new Apple()},
            //{ 44, new Banana()},
            //{ 45, new Buger()},
            //{ 46, new Cherries()},
            //{ 47, new Kiwi()},
            //{ 48, new Orange()},
            //{ 49, new Pear()},
            //{ 50, new Strawberry()},
        };

        // the level, xp and amount of skill points availible for the player
        public int lvl = 1;
        public float xp = 0;
        public int skillPoints = 0;

        // a lits of bought skills
        public List<int> UnlockedSkills = new List<int>() { };

        // an array of equipped skills
        public int[] equippedSkills =
        {
            -1,
            -1,
            -1,
        };

        // for checking if you use fireball in the overworld, so that lasse can get his fireball thing in
        // honestly kinda pointless, but if you beat the game (to the point we go aka not so far) then you
        // now know you can use fireball, in the overworld, its op
        public Keys? FireballKey
        {
            get
            {
                int? FireballIdx = null;
                for (int i = 0; i < equippedSkills.Length; i++)
                {
                    var CurSkillIdx = equippedSkills[i];
                    if(CurSkillIdx == -1)
                        continue;
                    if (Skill.Skills[CurSkillIdx] is Fireball)
                    {
            FireballIdx = i;
                        break;
                    }
                }

                switch(FireballIdx) {
            case 0:
                        return Keys.K_1;
            case 1:
                        return Keys.K_2;
            case 2:
                        return Keys.K_3;
                }


        return null;
            }
        }
    }
}
