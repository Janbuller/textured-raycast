using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    class RoofLight : Sprite, ILight
    {
        TexColor thisColor;
        float intesity;

        public RoofLight(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            effectedByLight = false;
            canInteract = false;
            thisColor = new TexColor(extraEffects[0], extraEffects[1], extraEffects[2]);
            intesity = (float)extraEffects[3]/100f;
        }

        public TexColor GetLightColor() {
            return thisColor;
        }

        public float GetLightIntensity() {
            return intesity;
        }
    }
}
