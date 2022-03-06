using System;
using System.Threading;
using System.Collections.Generic;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast
{
    class Program
    {
        static void Main(string[] args)
        {
            maze.input.InputManager.SetInputController(new maze.input.Windows.WindowsInputController());
            World world = new World();
            Maze.StartMaze(world);


            // var watch = new System.Diagnostics.Stopwatch();
            // watch.Start();
            // Texture test = TextureLoaders.loadFromPlainPPM("img/test4.ppm");
            // watch.Stop();
            // Console.Out.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
            // test.draw();

        }
    }
}
