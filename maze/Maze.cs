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
using textured_raycast.maze.DrawingLoops;

namespace textured_raycast.maze
{
    class Maze
    {
        static ConsoleBuffer GameBuffer;
        static ConsoleBuffer FightBuffer;
        static ConsoleBuffer UIBuffer;

        public static bool StartGame()
        {
            World.ce = new ConsoleEngine(World.WindowSize.Width, World.WindowSize.Height, "very dumb game");
            GameBuffer = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            FightBuffer = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            UIBuffer = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);

            Console.Clear();
            DrawScreen();

            return RunMainLoop();
        }

        private static bool RunMainLoop()
        {
            // Main game loop
            while (World.state != States.Stopping)
            {
                World.dt = (float)(DateTime.Now.Ticks - World.lastFrameTime) / TimeSpan.TicksPerSecond;
                World.lastFrameTime = DateTime.Now.Ticks;

                switch (World.state)
                {
                    case States.Fighting:
                        FightLoop.FightLoopIter(ref GameBuffer, ref FightBuffer, ref UIBuffer);
                        break;
                    case States.Inventory:
                        InventoryLoop.InventoryLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                    case States.Skills:
                        SkillsLoop.SkillsLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                    case States.Paused:
                        PauseLoop.PauseLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                    case States.Game:
                        GameLoop.GameLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                }

            }
            return false;
        }

        // Multi-threaded screen-drawing
        // =============================
        // Draw the screen asynchronously
        public static void DrawScreen()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    World.ce.DrawScreen();
                }
            });
        }

    }
}
