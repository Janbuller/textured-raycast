using System;
using System.Collections.Generic;
using textured_raycast.maze.skills.Skills;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.skills
{
    abstract class Skill
    {
	// The name of the skill
        public string name;
	// The id of the skills texture.
        public int TexID;

	// Description of the skill
        public string Desc;

	// Price of the skill
        public int price;
	// The idx of all other skills, that need to be purchased first.
        public int[] requiredSkills;

	// This skill id.
        public int id;

        public Skill(int id, int price, int[] requiredSkills)
        {
            this.id = id;
            this.price = price;
            this.requiredSkills = requiredSkills;
        }

	// Runs, when the player tries to buy the skill
        public void TryBuy()
        {
	    // Can't buy, if it's too expensive.
            if (price > World.player.skillPoints)
                return;

	    // Can't buy if already owned.
            if (isBought())
                return;

	    // Whether all requiredskills are owned. True by default,
	    // set to false if not.
            bool allOwned = true;

	    // Reference to all unlocked skills.
            var UnlockedSkills = World.player.UnlockedSkills;
	    // If this skill actually requires others to be purchased.
            if (requiredSkills != null)
            {
		// Loop through each required skill
                foreach (int skill in requiredSkills)
                {
		    // If the required skill is not in unlocked
		    // skills, allowned is false.
                    if (!(UnlockedSkills.Contains(skill)))
                    {
                        allOwned = false;
                    }
                }   

		// Can't buy if requirements aren't met.
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

	    // Price is subtracted from the player and the skill is
	    // added.
            World.player.skillPoints -= price;
            World.player.UnlockedSkills.Add(this.id);
        }

	// Check if the skill is already bought.
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

	// Get the texture of the skill.
        public Texture getTexture()
        {
	    // Poll the resourcemanager for the standard texture.
            Texture standardTex = ResourceManager.getTexture(SkillTextures[TexID]);

	    //If the skill is already bought, return the standard
	    //texture, otherwise, return a greyedout version.
            if (isBought())
            {
                return standardTex;
            }
            else
            {
		// Try to get the greyedout texture from the
		// resourcemanager. If it exists, return it,
		// otherwise, make it and cache it in the
		// resourcemanager.
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
            {
                World.player.equippedSkills[assignToNr] = id;
            }
        }

	// A list of all the textures for skills
        public static Dictionary<int, string> SkillTextures = new Dictionary<int, string>()
        {
            {1, "img/skills/tmp-slash-skill.ppm"},
            {2, "img/skills/fireball.ppm"},
            {3, "img/skills/lifesteal.ppm"},
            {4, "img/skills/Poison.ppm"},
            {5, "img/skills/MagicPower.ppm"},
        };

        // the dictionarry that represents all slots in the skill tree
        public static Dictionary<int, Skill> Skills = new Dictionary<int, Skill>()
        {
            {0,  new Slash(0, 2, new int[]{2})},
            {1,  new Bezerk(1, 10, new int[]{2, 10})},
            {2,  new DoubleOrNothing(2, 10, new int[]{5})},
            {3,  new Poison(3, 10, new int[]{2, 14})},
            {4,  new Phys50(4, 32, new int[]{7})},
            {5,  new LS10(5, 4, new int[]{12})},
            {6,  new Mag50(6, 32, new int[]{17})},
            {7,  new Phys40(7, 16, new int[]{8})},
            {8,  new RecklessSlash(8, 0, new int[]{9})},
            {9,  new Phys30(9, 8, new int[]{10})},
            {10, new Phys20(10, 4, new int[]{11})},
            {11, new Phys10(11, 2, new int[]{12})},
            {12, new Slash(12, 0, null)},
            {13, new Mag10(13, 2, new int[]{12})},
            {14, new Mag20(14, 4, new int[]{13})},
            {15, new Mag30(15, 8, new int[]{14})},
            {16, new Fireball(16, 0, new int[]{15})},
            {17, new Mag40(17, 16, new int[]{16})},
            {18, new Phys50(18, 32, new int[]{7})},
            {19, new LS10(19, 4, new int[]{12})},
            {20, new Mag50(20, 32, new int[]{17})},
            {21, new DodgeStance(21, 10, new int[]{22, 10})},
            {22, new Both20(22, 10, new int[]{19})},
            {23, new ChargeSpell(23, 10, new int[]{22, 14})},
            {24, new Slash(24, 2, new int[]{22})},
        };
    }
}
