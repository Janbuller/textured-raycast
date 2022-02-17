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
        public int spriteID;
        public int texID;
        public bool canInteract = false;
        public int effectID;
        public int extraEffectDetailID;

        public Sprite(double posX, double posY, int spriteID, int effectID = 0, int extraEffectDetailID = 0)
        {
            this.pos = new Vector2d(posX, posY);
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
            else if (effectID == 3) // or maby a door, fx (it could also be an invisible door, so just a tp point, and then have a door image on the wall)
            {
                // go to map with id extraEffectDetailID
                // or maby more like opening of the door with id extraEffectDetailID
                // so that if you enter one plae, you exit the same place
            }
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
