using System;
using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
using textured_raycast.maze.texture;
using textured_raycast.maze.skills;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using System.Threading.Tasks;
using textured_raycast.maze.ButtonList;
using textured_raycast.maze.ButtonList.Buttons.INV;
using textured_raycast.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.DrawingLoops
{
    class FightLoop
    {
        public static void FightLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer fight, ref ConsoleBuffer UIHolder)
        {
            World.fight.tillFightBegins -= (float)World.dt;

            if (World.fight.tillFightBegins < 0)
            {
                fight.Clear();

                if (InputManager.GetKey(Keys.K_1) == KeyState.KEY_DOWN)
                    World.player.useSkill(0);
                if (InputManager.GetKey(Keys.K_2) == KeyState.KEY_DOWN)
                    World.player.useSkill(1);
                if (InputManager.GetKey(Keys.K_3) == KeyState.KEY_DOWN)
                    World.player.useSkill(2);

                World.fight.renderFightToBuffer(ref fight);

                World.ce.DrawConBuffer(fight);
                World.ce.SwapBuffers();
            }
            else
            {
                UIHolder.Clear();
                World.fight.renderFightStartScreenToBuffer(ref UIHolder, World.fight.tillFightBegins / 2 - 0.1f);

                World.ce.DrawConBuffer(game.mixBuffer(UIHolder));
                World.ce.SwapBuffers();
            }
        }
    }
}
