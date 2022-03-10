using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.sprites.allSprites
{
    class RotatingSprite : Sprite
    {
        Vector2d dir = new Vector2d(1, 0);

        public RotatingSprite(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            canInteract = false;
        }

        public override void updateAnimation(float dt)
        {

        }

        public override void UpdateOnDraw(ref World world, double distToPlayer)
        {
            List<Texture> textures = Sprite.IDTextureCorrespondence[texID];
            int textureCount = textures.Count;
            double radPrTex = (Math.PI * 2) / textureCount;

            double sprRot = Math.Atan2(dir.x, dir.y);
            double plrRot = Math.Atan2(world.plrRot.x, world.plrRot.y);

            double rotDiff = (sprRot - plrRot) + radPrTex/2;
            if(rotDiff < 0)
                rotDiff += Math.PI*2;
            if(rotDiff > Math.PI*2)
                rotDiff -= Math.PI*2;

            curTexture = (int)(rotDiff / radPrTex);
        }
    }
}
