using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;

namespace textured_raycast.maze.sprites.allSprites
{
    class Button : Sprite
    {
        public Button(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void Activate(ref World world)
        {
            world.getMapByID(1).SetCell(15, 6, 0);
            world.currentMessage = "you heard a *click* sound";
        }

        public override string ActivateMessage()
        {
            return "Press the button?";
        }
    }
}
