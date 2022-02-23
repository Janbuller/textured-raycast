using System;
using System.Threading;
using System.Collections.Generic;
using textured_raycast.maze;
using textured_raycast.maze.texture;

namespace textured_raycast
{
    class Program
    {
        static void Main(string[] args)
        {
            maze.input.InputManager.SetInputController(new maze.input.console.ConsoleInputController());
            World world = new World();
            Maze.StartMaze(world);


            // Texture test = TextureLoaders.loadFromPlainPPM("img/test.ppm");
            // test.draw();

        }
    }
}
