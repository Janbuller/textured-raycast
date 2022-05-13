namespace textured_raycast.maze.ButtonList.Buttons.INV
{
    class Skills : Button
    {
        public Skills(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list) {}

        public override void onActivate()
        {
            // open skill loop
            World.state = States.Skills;
        }
    }
}
