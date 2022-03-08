using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;
using System.Linq;

namespace textured_raycast.maze.sprites.allSprites
{
    class Portal : Sprite
    {

        Texture portalTex;

        public Portal(double posX, double posY, int spriteID, int effectID = 0, string whatsLeft = "") : base(posX, posY, spriteID, effectID, whatsLeft)
        {
            define(posX, posY, spriteID, effectID, whatsLeft);
        }

        public override void onLoad()
        {
            interactDistance = 0.4f;
            canInteract = true;
            autoInteract = true;
        }

        public override void Activate(ref World world)
        {
            world.plrPos += new Vector2d(extraEffects[0], extraEffects[1]);
        }

        public override void Update(ref World world, float dt)
        {
            Texture startTex = IDTextureCorrespondence[texID][0];
            portalTex = new Texture(startTex);
            Map curMap = world.getMapByID(world.currentMap);

            Vector2d dir = world.plrRot;
            Vector2d plane = new Vector2d(dir.y, -dir.x) * 0.66;
            for(int x = 0; x < portalTex.width; x++) {
                Maze.WallcastReturn wcr = Maze.DoOneWallcast(x, portalTex.width, portalTex.height, curMap.GetLights(), dir, plane, world.plrPos + new Vector2d(extraEffects[0], extraEffects[1]), 1, curMap);

                if (wcr.HitWall.doDraw) {
                    if(curMap.GetLights().Count() > 0) {
                        TextureHelper.DrawVerLine(ref portalTex, x, wcr.LineHeight, wcr.Tex, wcr.TexX, wcr.Darken, wcr.MixedLight, new TexColor(255, 0, 255));
                    } else {
                        TextureHelper.DrawVerLine(ref portalTex, x, wcr.LineHeight, wcr.Tex, wcr.TexX, wcr.Darken, new TexColor(255, 0, 255));
                    }
                }
            }
        }

        public override Texture GetTexture() {
            return portalTex;
        }
    }
}
