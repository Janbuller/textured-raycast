using System.Runtime.InteropServices;
using textured_raycast.maze;
using textured_raycast.maze.online;
using textured_raycast.maze.OpenGL;
using ENet;

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
	    else
                maze.input.InputManager.SetInputController(new maze.input.console.ConsoleInputController());

            World.setupMapsInWorld();
            Maze.StartGame();
        }
    }
}
