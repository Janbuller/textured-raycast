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
        Vector2<double> pos;
        public int spriteID;
        public int texID;
        public bool canInteract = false;
        public int effectID;
        public int extraEffectDetailID;

        public Sprite(double posX, double posY, int spriteID, int effectID = 0, int extraEffectDetailID = 0)
        {
            this.pos = new Vector2<double>(posX, posY);
            this.spriteID = spriteID;
            this.texID = spriteID;
            if (effectID != 0)
                this.canInteract = true;
            this.effectID = effectID;
            this.extraEffectDetailID = extraEffectDetailID;
        }

        public void Activate()
        {
            if (effectID == 2) // is a chest, fx
            {
                // add item id extraEffectDetailID to player inventory
            }
            else if (effectID == 3) // or maby a door, fx
            {
                // go to map with id extraEffectDetailID
            }
        }

        public Vector2<double> getPos() {
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
