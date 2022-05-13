using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    class LightSprite : Sprite, ILight
    {
        // R(0-255), G(0-255), B(0-255), Intensity(0-100), Linear, Quadratic
        TexColor thisColor;
        float intesity;

        float linear;
        float quadratic;

        public LightSprite(double posX, double posY, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            // light cant be effected by light
            // and you cant interact with it...
            effectedByLight = false;
            canInteract = false;
            // set the color to what we gave in map editor, same with intensity, kubear and quadratics
            // all stuff for calculating light strength based on distance
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
