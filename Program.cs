using System.Runtime.InteropServices;
using textured_raycast.maze;

namespace textured_raycast
{
    class Program
    {
        static void Main(string[] args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                maze.input.InputManager.SetInputController(new maze.input.linux.LinuxInputController());
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                maze.input.InputManager.SetInputController(new maze.input.Windows.WindowsInputController());

            World.setupMapsInWorld();
            Maze.StartGame();

            // var watch = new System.Diagnostics.Stopwatch();
            // watch.Start();
            // Texture test = TextureLoaders.loadFromPlainPPM("img/test4.ppm");
            // watch.Stop();
            // Console.Out.WriteLine($"Time: {watch.ElapsedMilliseconds} ms");
            // test.draw();

        }
    }
}
