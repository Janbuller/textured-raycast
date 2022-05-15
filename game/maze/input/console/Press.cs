
namespace textured_raycast.maze.input.console
{
    // Holds info about a consolekey press.
    class Press
    {
        public KeyState State;
        public long Time;

        public Press(KeyState State, long Time)
        {
            this.State = State;
            this.Time = Time;
        }
    }
}
