using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.skills;
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

namespace rpg_game.maze.ButtonList.Buttons.Skills
{
    class SkillPlaceHolder : Button
    {
        static int curId;
        public int id;

        public SkillPlaceHolder(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list)
        {
            id = SkillPlaceHolder.curId;
            SkillPlaceHolder.curId += 1;
        }

        override public void onActivate()
        {
            Skill.Skills[id].TryBuy();
        }

        public void assignSkill(int assignToNr)
        {
            Skill.Skills[id].assignTo(assignToNr);
        }

        public bool isBought()
        {
            return Skill.Skills[id].isBought();
        }
    }
}
