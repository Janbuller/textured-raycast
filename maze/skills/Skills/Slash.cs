using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.GUI;
using System.Threading.Tasks;
using rpg_game.maze;
using rpg_game.maze.ButtonList.Buttons.INV;
using rpg_game.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.skills.Skills
{
    class Slash : Skill, IActiveSkill
    {
        public Slash(int id, int price, int[] requiredSkills) : base(id, price, requiredSkills)
        {
	        name = "Slash";
	        TexID = 1;

	        requiredSkills = new int[0];

	        ResourceManager.getTexture("slhfsld");
        }

        public void Activate()
        {
            World.fight.hp -= World.player.dam;
        }
    }
}
