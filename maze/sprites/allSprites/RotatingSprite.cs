using System;
using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites.allSprites
{
    class RotatingSprite : Sprite
    {
        Vector2d dir = new Vector2d(-1, 0);
        double rot = 0;

        public RotatingSprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            canInteract = false;
        }

        public override void updateAnimation()
        {
            // rot += dt*2;
            while(rot > Math.PI)
                rot -= Math.PI*2;
            while(rot < -Math.PI)
                rot += Math.PI*2;
            dir.x = Math.Cos(rot+Math.PI);
            dir.y = Math.Sin(rot+Math.PI);
        }

        public override void UpdateOnDraw(double distToPlayer)
        {
            List<string> texturePaths = Sprite.IDTextureCorrespondence[texID];
            Texture[] textures = new Texture[texturePaths.Count];
            for (int i = 0; i < textures.Length; i++)
            {
                textures[i] = ResourceManager.getTexture(texturePaths[i]);
            }

            int textureCount = textures.Length;
            double radPrTex = (Math.PI * 2) / textureCount;

            double sprRot = Math.Atan2(dir.x, dir.y);
            double plrRot = Math.Atan2(World.plrRot.x, World.plrRot.y);

            double rotDiff = ((sprRot - plrRot) + radPrTex/2) - Math.PI;

            while(rotDiff < 0)
                rotDiff += Math.PI*2;
            while(rotDiff > Math.PI*2)
                rotDiff -= Math.PI*2;

            if (rotDiff > Math.PI)
            {
                rot -= 0.0005f;
            } else if (rotDiff < Math.PI)
            {
                rot += 0.0005f;
            }
            curTexture = (int)(rotDiff / radPrTex);
        }
    }
}
