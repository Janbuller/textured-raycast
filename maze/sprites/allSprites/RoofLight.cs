using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    class RoofLight : Sprite, ILight
    {
        TexColor thisColor;
        float intesity;

        float linear;
        float quadratic;

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

            if (extraEffects.Count > 4)
            {
                linear = (float)extraEffects[4] / 100;
                quadratic = (float)extraEffects[5] / 100;
            } else {
                linear = 0.22f;
                quadratic = 0.20f;
	    }
        }

        public TexColor GetLightColor() {
            return thisColor;
        }

        public float GetLightIntensity() {
            return intesity;
        }

        public Vector2d GetLightPos() {
            return pos;
        }

        public float GetAttenuationLinear()
        {
	    return linear;
        }

        public float GetAttenuationQuadratic()
        {
            return quadratic;
        }
    }
}
