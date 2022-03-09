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
            Texture startTex = IDTextureCorrespondence[texID][0];
            portalTex = new Texture(startTex);

            interactDistance = 0.2f;
            canInteract = true;
            autoInteract = true;
        }

        public override void Activate(ref World world)
        {
            world.plrPos += new Vector2d(extraEffects[0], extraEffects[1]);
        }

        public override void UpdateOnDraw(ref World world, double distToPlayer)
        {
            Texture startTex = IDTextureCorrespondence[texID][0];
            portalTex = new Texture(startTex);
            Map curMap = world.getMapByID(world.currentMap);

            Vector2d portalLocOffset = new Vector2d(extraEffects[0], extraEffects[1]);
            Vector2d tpToLoc = world.plrPos + portalLocOffset;

            if(tpToLoc.x > curMap.Width-1 || tpToLoc.x < 1 || tpToLoc.y > curMap.Height-1 || tpToLoc.y < 1) {
                // for(int y = 0; y < portalTex.width; y++) {
                //     for(int x = 0; x < portalTex.height; x++) {
                //         if(portalTex.getPixel(x, y) == new TexColor(255, 0, 255))
                //             portalTex.setPixel(x, y, new TexColor(0, 0, 0));
                //     }
                // }
                // return;

                tpToLoc = getPos() + portalLocOffset;
            }

            Vector2d dir = world.plrRot;
            Vector2d plane = new Vector2d(dir.y, -dir.x) * 0.66;

            Texture FloorAndRoof = new Texture(portalTex.width, portalTex.height);
            Maze.FloorCasting(ref FloorAndRoof, dir, plane, pos, 1, curMap, world);

            for(int x = 0; x < portalTex.width; x++) {
                Maze.WallcastReturn wcr = Maze.DoOneWallcast(x, portalTex.width, portalTex.height, curMap.GetLights(), dir, plane, tpToLoc, 1, curMap);
                int LineHeight = (int)((portalTex.height * 2) / (wcr.PerpWallDist + 1/distToPlayer));

                if (wcr.HitWall.doDraw) {
                    if(curMap.GetLights().Count() > 0) {
                        TextureHelper.DrawVerLine(ref portalTex, x, LineHeight, wcr.Tex, wcr.TexX, wcr.Darken, wcr.MixedLight, new TexColor(255, 0, 255));
                    } else {
                        TextureHelper.DrawVerLine(ref portalTex, x, LineHeight, wcr.Tex, wcr.TexX, wcr.Darken, new TexColor(255, 0, 255));
                    }
                }

                TextureHelper.DrawVerLine(ref portalTex, x, portalTex.height, FloorAndRoof, x, 0.00001f, new TexColor(255, 255, 255), new TexColor(255, 0, 255));
            }
        }

        public override Texture GetTexture() {
            return portalTex;
        }
    }
}
