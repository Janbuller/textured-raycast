namespace textured_raycast.maze.ButtonList.Buttons.INV
{
    class Back : Button
    {
        public Back(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list) {}

        public override void onActivate()
        {
            World.state = states.Paused;
        }
    }
}
