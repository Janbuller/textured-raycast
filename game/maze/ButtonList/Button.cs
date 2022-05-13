using textured_raycast.maze.texture;

namespace textured_raycast.maze.ButtonList
{
    abstract class Button
    {
        // x y w and h of the button
        public int x;
        public int y;
        public int w;
        public int h;

        // list of what happens to corresponding key presses, for navigation between buttons
        public int[] listOfMovements;

        public Button(int x, int y, int w, int h, int[] list)
        {
            // define the values
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            this.listOfMovements = list;
        }

        public ConsoleBuffer DrawOnBuffer(ConsoleBuffer editBuffer)
        {
            // add a slight clor change to an area, for highliting buttons
            for (int thisX = x; thisX < w + x; thisX++)
            {
                for (int thisY = y; thisY < h + y; thisY++)
                {
                    editBuffer.DrawPixel(editBuffer.GetPixel(thisX, thisY) + new TexColor(50, 50, 0), thisX, thisY);
                }
            }

            return editBuffer;
        }

        virtual public void onActivate()
        {
            // what happens when you activate it
        }
    }
}
