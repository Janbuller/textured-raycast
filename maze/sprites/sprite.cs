using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites
{
    internal class Sprite
    {
        Vector2d pos;
        public int texID;
        public Sprite(Vector2d pos, int texID)
        {
            this.pos = pos;
            this.texID = texID;
        }

        public Sprite(double posX, double posY, int texID)
        {
            this.pos = new Vector2d(posX, posY);
            this.texID = texID;
        }

        public Vector2d getPos() {
            return pos;
        }
        public double getX() {
            return pos.x;
        }
        public double getY() {
            return pos.y;
        }
    }
}
