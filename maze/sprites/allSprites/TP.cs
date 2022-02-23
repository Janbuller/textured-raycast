using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class TP : Sprite
    {
        public TP(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void Activate(ref World world)
        {
            world.plrPos = new math.Vector2d(1.5, 18.5);
        }

        public override string ActivateMessage()
        {
            return "Is this some sort of teleportation device?";
        }
    }
}
