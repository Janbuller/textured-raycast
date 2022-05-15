using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
namespace textured_raycast.maze.OpenGL
{
    public static class MainGL
    {
        static Window win;

        public static void MainRun()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(480, 320),
                Title = World.ce.Title,
                Flags = ContextFlags.ForwardCompatible,
        // WindowBorder = WindowBorder.Fixed
            };

        using (win = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                win.Run();
            }
        }

        public static void MainStop()
        {
            win.Close();
        }
    }
}
