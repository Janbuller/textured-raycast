using System;
using System.Collections.Generic;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.sprites.allSprites
{
    class PlayerSprite : Sprite
    {
        // this is for online.
        Vector2d dir = new Vector2d(-1, 0); 

        public PlayerSprite(double posX, double posY, double xRot, double yRot, string[] texture, int effectID = 0, string whatsLeft = "") : base(posX, posY, texture, effectID, whatsLeft)
        {
            define(posX, posY, texture, effectID, whatsLeft);

            // set the rotation
            dir = new Vector2d(xRot, yRot);
        }

        public override void onLoad()
        {
            canInteract = false; // you cant talk to other players
        }

        public override Texture GetTexture()
        {
            // get the texture for the player based on rotation of the player, and rotation of yourself
            // the player has 8 images for 8 different angles

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
