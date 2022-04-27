using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
namespace textured_raycast.maze.OpenGL
{
    public static class MainGL
    {
        public static void MainRun()
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK - Creating a Window",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
            };

	    using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }
    }
}
