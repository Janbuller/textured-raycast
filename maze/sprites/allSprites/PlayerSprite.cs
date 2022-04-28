using System;
using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites.allSprites
{
    class PlayerSprite : Sprite
    {
        Vector2d dir = new Vector2d(-1, 0);

        public PlayerSprite(double posX, double posY, double xRot, double yRot, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);

            dir = new Vector2d(xRot, yRot);
        }

        public override void onLoad()
        {
            canInteract = false;
        }

        virtual public Texture GetTexture()
        {
            int textureCount = texture.Length;
            double radPrTex = (Math.PI * 2) / textureCount;

            double sprRot = Math.Atan2(dir.X, dir.Y);
            double plrRot = Math.Atan2(World.plrRot.X, World.plrRot.Y);

            double rotDiff = ((sprRot - plrRot) + radPrTex / 2) - Math.PI;

            while (rotDiff < 0)
                rotDiff += Math.PI * 2;
            while (rotDiff > Math.PI * 2)
                rotDiff -= Math.PI * 2;

            return ResourceManager.getTexture(texture[(int)(rotDiff / radPrTex)]);
        }
    }
}
