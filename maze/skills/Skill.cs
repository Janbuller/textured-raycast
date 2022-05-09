using System.Collections.Generic;
using textured_raycast.maze.skills.Skills;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills
{
    abstract class Skill
    {
        public string name;
        public int TexID;

        public string Desc;

        public int price;
        public int[] requiredSkills;

        public int id;

        public Skill(int id, int price, int[] requiredSkills)
        {
            this.id = id;
            this.price = price;
            this.requiredSkills = requiredSkills;
        }

        public void TryBuy()
        {
            if (price > World.player.skillPoints)
                return;

            if (isBought())
                return;

            bool allOwned = true;

            var UnlockedSkills = World.player.UnlockedSkills;
            if (requiredSkills != null)
            {
                foreach (int skill in requiredSkills)
                {
                    if (!(UnlockedSkills.Contains(skill)))
                    {
                        allOwned = false;
                    }
                }   

                if (!allOwned)
                    return;
            }

            if (this is IPassiveSkill)
            {
                World.player.addPMag += (this as IPassiveSkill).getMagP();
                World.player.addPPhys += (this as IPassiveSkill).getStrP();
            }
            if (this is IPassiveSkillLS)
                World.player.addPLifeSteal += (this as IPassiveSkillLS).lifeSteal();

            if (this is IPassiveSkillDON)
                World.player.don = true;

            World.player.skillPoints -= price;
            World.player.UnlockedSkills.Add(this.id);
        }

        public bool isBought()
        {
            var UnlockedSkills = World.player.UnlockedSkills;
            foreach (int skill in UnlockedSkills)
            {
                if (skill == id)
                    return true;
            }
            return false;
        }

        public Texture getTexture()
        {
            Texture standardTex = ResourceManager.getTexture(SkillTextures[TexID]);
            if (isBought())
            {
                return standardTex;
            }
            else
            {
                Texture greyedOut = ResourceManager.getTexture(SkillTextures[TexID] + "-progGrey");
                if (greyedOut != null)
                    return greyedOut;
                greyedOut = standardTex.getGreyscale();
                ResourceManager.cacheTexture(SkillTextures[TexID] + "-progGrey", greyedOut);
                return greyedOut;
            }
        }

        public void assignTo(int assignToNr)
        {
            if (isBought())
                World.player.equippedSkills[assignToNr] = id;
        }

        public static Dictionary<int, string> SkillTextures = new Dictionary<int, string>()
        {
            {1, "img/skills/tmp-slash-skill.ppm"}
        };

        // the dictionarry that represents all slots in the skill tree
        public static Dictionary<int, Skill> Skills = new Dictionary<int, Skill>()
        {
            {0,  new Slash(0,  2, new int[]{2})},
            {1,  new Bezerk(1,  2, new int[]{2, 10})},
            {2,  new DoubleOrNothing(2,  2, new int[]{5})},
            {3,  new Poison(3,  2, new int[]{2, 14})},
            {4,  new Phys50(4,  2, new int[]{7})},
            {5,  new LS10(5,  2, new int[]{12})},
            {6,  new Mag50(6,  2, new int[]{17})},
            {7,  new Phys40(7,  2, new int[]{8})},
            {8,  new Slash(8,  2, new int[]{9})},
            {9,  new Phys30(9,  2, new int[]{10})},
            {10, new Phys20(10, 2, new int[]{11})},
            {11, new Phys10(11, 2, new int[]{12})},
            {12, new Slash(12, 1, null)},
            {13, new Mag10(13, 2, new int[]{12})},
            {14, new Mag20(14, 2, new int[]{13})},
            {15, new Mag30(15, 2, new int[]{14})},
            {16, new Slash(16, 2, new int[]{15})},
            {17, new Mag40(17, 2, new int[]{16})},
            {18, new Phys50(18, 2, new int[]{7})},
            {19, new LS10(19, 2, new int[]{12})},
            {20, new Mag50(20, 2, new int[]{17})},
            {21, new DodgeStance(21, 2, new int[]{22, 10})},
            {22, new Both20(22, 2, new int[]{19})},
            {23, new ChargeSpell(23, 2, new int[]{22, 14})},
            {24, new Slash(24, 2, new int[]{22})},
        };
    }
}
