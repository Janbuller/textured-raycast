using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze;
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
        string name;
        int TexID;

        int price;
        int[] requiredSkills;
    }

    class SkillClass
    {
        public static Dictionary<int, string> SkillTextures = new Dictionary<int, string>()
	{
	    
	};

        public static Dictionary<int, Skill> Skills = new Dictionary<int, Skill>()
	{
	    
	};
    }
}
