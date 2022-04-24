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
        static ConsoleBuffer game;
        static ConsoleBuffer fight;
        static ConsoleBuffer UIHolder;

        public static bool StartMaze()
        {
            World.ce = new ConsoleEngine(World.size.x, World.size.y, "very dumb game");
            game = new ConsoleBuffer(World.size.x, World.size.y);
            fight = new ConsoleBuffer(World.size.x, World.size.y);
            UIHolder = new ConsoleBuffer(World.size.x, World.size.y);

            Console.Clear();
            DrawScreen();

            return Start();
        }

        private static bool Start()
        {
            // Main game loop
            while (World.state != States.Stopping)
            {
                World.dt = (float)(DateTime.Now.Ticks - World.lastFrameTime) / TimeSpan.TicksPerSecond;
                World.lastFrameTime = DateTime.Now.Ticks;

                switch (World.state)
                {
                    case States.Fighting:
                        FightLoop.FightLoopIter(ref game, ref fight, ref UIHolder);
                        break;
                    case States.Inventory:
                        InventoryLoop.InventoryLoopIter(ref game, ref UIHolder);
                        break;
                    case States.Skills:
                        SkillsLoop.SkillsLoopIter(ref game, ref UIHolder);
                        break;
                    case States.Paused:
                        PauseLoop.PauseLoopIter(ref game, ref UIHolder);
                        break;
                    case States.Game:
                        GameLoop.GameLoopIter(ref game, ref UIHolder);
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
