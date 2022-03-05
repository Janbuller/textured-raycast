using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    class RoofLight : Sprite
    {
        public TexColor thisColor;
        public int intesity;

        public RoofLight(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            canInteract = false;
            thisColor = new TexColor(extraEffects[0]*255, extraEffects[1]*255, extraEffects[2]*255);
            intesity = extraEffects[3]/9;
        }
    }
}
