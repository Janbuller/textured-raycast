using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.texture;

namespace rpg_game.maze
{
    abstract class Button
    {
        public int x;
        public int y;
        public int w;
        public int h;

        public int[] listOfMovements;

        public Button(int x, int y, int w, int h, int[] list)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;

            this.listOfMovements = list;
        }

        public ConsoleBuffer DrawOnBuffer(ConsoleBuffer editBuffer)
        {
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

        }
    }
}
