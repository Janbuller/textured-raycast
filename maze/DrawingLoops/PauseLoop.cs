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
    class PauseLoop
    {
        public static void PauseLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
	    UIHolder.Clear();
	    
	    GUI.GUI.pauseGUI(ref UIHolder);
	    
	    World.ce.DrawConBuffer(game.mixBuffer(UIHolder));
	    World.ce.SwapBuffers();
        }
    }
}
