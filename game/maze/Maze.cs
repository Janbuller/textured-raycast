using System;
using System.Threading.Tasks;
using textured_raycast.maze.DrawingLoops;
using textured_raycast.maze.online;
using textured_raycast.maze.OpenGL;

namespace textured_raycast.maze
{
    class Maze
    {
        static bool usingGL = true;

        static ConsoleBuffer GameBuffer;
        static ConsoleBuffer FightBuffer;
        static ConsoleBuffer UIBuffer;

    public static bool StartGame()
        {
            World.ce    = new ConsoleEngine(World.WindowSize.Width, World.WindowSize.Height, "The Lands Above");
            GameBuffer  = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            FightBuffer = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);
            UIBuffer    = new ConsoleBuffer(World.WindowSize.Width, World.WindowSize.Height);

            World.fight = new Fight.Fight();
            World.fight.link(ref GameBuffer, ref FightBuffer, ref UIBuffer);

            Console.Clear();
            DrawScreen();
            StartClientPlayer();


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
                        // it is no longer supposed to enter this
                        // state while in the main loop, so if it
                        // somehow does, return it.
                        World.state = States.Game; 
                        break;
                    case States.Inventory:
                        InventoryLoop.InventoryLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                    case States.Skills:
                        SkillsLoop.SkillsLoopIter(ref GameBuffer, ref UIBuffer);
                        break;
                    case States.Settings:
                        if (usingGL)
                            MainGL.MainStop();

                        usingGL = !usingGL;
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
                    while (!usingGL)
                    {
                        Console.WriteLine("b");
                        World.ce.DrawScreen();
                    }

                    MainGL.MainRun();
                }
            });
        }

        // Multi-threaded multi-player
        // =============================
        public static void StartClientPlayer()
        {
           Task.Run(() =>
            {
                Client.Start("10.29.131.194", 4231);
            });
        }

    }
}
