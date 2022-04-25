using System;
using System.Threading.Tasks;
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
            World.ce =    new ConsoleEngine(World.WindowSize.Width, World.WindowSize.Height, "very dumb g̶a̶m̶e̶");
            GameBuffer =  new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            FightBuffer = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            UIBuffer =    new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);

            Console.Clear();
            DrawScreen();

            return RunMainLoop();
        }

        private static bool RunMainLoop()
        {
            // Main game loop
            while (World.state != States.Stopping)
            {
                // Calculate and set deltatime in world.
                World.UpdateDeltaTime();

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
		    case States.Settings:
                        World.state = States.Paused;
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
