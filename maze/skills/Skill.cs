using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze;
using textured_raycast.maze.skills.Skills;
using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.GUI;
using textured_raycast.maze.resources;
using System.Threading.Tasks;
using rpg_game.maze;
using rpg_game.maze.ButtonList.Buttons.INV;
using rpg_game.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.skills
{
    abstract class Skill
    {
        public string name;
        public int TexID;

        public int price;
        public int[] requiredSkills;

        public int id;

	public Skill(int id, int price, int[] requiredSkills) {
	    this.id = id;
	    this.price = price;
            this.requiredSkills = requiredSkills;
        }

        public void TryBuy()
        {
            if (price > World.player.skillPoints)
                return;

	    if(isBought())
		return;

            int fufilledReqs = 0;

            var UnlockedSkills = World.player.UnlockedSkills;
            if (requiredSkills != null)
            {
                foreach (int skill in requiredSkills)
                {
                    if (UnlockedSkills.Contains(skill))
                    {
                        fufilledReqs++;
                    }
                }

                if (fufilledReqs != requiredSkills.Count())
                    return;
            }
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
	    if(isBought()) {
                return standardTex;
            }
	    else
            {
                Texture greyedOut = ResourceManager.getTexture(SkillTextures[TexID] + "-progGrey");
		if(greyedOut != null)
		    return greyedOut;
                greyedOut = standardTex.getGreyscale();
		ResourceManager.cacheTexture(SkillTextures[TexID] + "-progGrey", greyedOut);
                return greyedOut;
            }
        }

        public static Dictionary<int, string> SkillTextures = new Dictionary<int, string>()
	{
	    {1, "img/skills/tmp-slash-skill.ppm"}
	};

        public static Dictionary<int, Skill> Skills = new Dictionary<int, Skill>()
	{
	    {0,  new Slash(0,  2, new int[]{2})},
	    {1,  new Slash(1,  2, new int[]{2,10})},
	    {2,  new Slash(2,  2, new int[]{0,1,3,5})},
	    {3,  new Slash(3,  2, new int[]{2,14})},
	    {4,  new Slash(4,  2, new int[]{7})},
	    {5,  new Slash(5,  2, new int[]{2,12})},
	    {6,  new Slash(6,  2, new int[]{17})},
	    {7,  new Slash(7,  2, new int[]{4, 8, 18})},
	    {8,  new Slash(8,  2, new int[]{7, 9})},
	    {9,  new Slash(9,  2, new int[]{8, 10})},
	    {10, new Slash(10, 2, new int[]{1, 9, 11, 21})},
	    {11, new Slash(11, 2, new int[]{10,12})},
	    {12, new Slash(12, 2, null)},
	    {13, new Slash(13, 2, new int[]{12,14})},
	    {14, new Slash(14, 2, new int[]{3,13,15,23})},
	    {15, new Slash(15, 2, new int[]{14,16})},
	    {16, new Slash(16, 2, new int[]{15,17})},
	    {17, new Slash(17, 2, new int[]{6,16,20})},
	    {18, new Slash(18, 2, new int[]{7})},
	    {19, new Slash(19, 2, new int[]{12,22})},
	    {20, new Slash(20, 2, new int[]{17})},
	    {21, new Slash(21, 2, new int[]{10,22})},
	    {22, new Slash(22, 2, new int[]{12,21,23})},
	    {23, new Slash(23, 2, new int[]{14,22})},
	    {24, new Slash(24, 2, new int[]{22})},
	};
    }
}
