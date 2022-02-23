using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class Barrel : Sprite
    {
        public Barrel(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void Activate(ref World world)
        {
            if (texID == 8)
            {
                texID = 10;
            }
            else
            {
                world.getMapByID(1).SetCell(16, 5, 0);
                world.currentMessage = "you heard a *click* sound";
                canInteract = false;
            }
        }

        public override string ActivateMessage()
        {
            if (texID == 8)
                return "Break the barrel";
            else
                return "Theres a button under the barrel, press it?";
        }
    }
}
