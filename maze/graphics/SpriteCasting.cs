using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.GUI;
using textured_raycast.maze.resources;
using System.Threading.Tasks;
using rpg_game.maze;
using rpg_game.maze.ButtonList.Buttons.INV;
using rpg_game.maze.ButtonList.Buttons.Skills;

namespace textured_raycast.maze.graphics
{
    class SpriteCasting
    {
        public static void SpriteCast(ref ConsoleBuffer game, List<Sprite> sprites, Vector2d pos, Vector2d plane, Vector2d dir, double[] ZBuffer, int visRange, Map map)
        {
            ILight[] lights = map.GetLights();

            List<double> spriteDist = new List<double>();
            for (int i = 0; i < sprites.Count; i++)
            {
                // Calculate sprite distance from player, using pythagoras.
                // Since it's only used for comparing with itself, sqrt isn't required.
                double xDist = pos.x - sprites[i].getX();
                double yDist = pos.y - sprites[i].getY();

                spriteDist.Add(xDist * xDist + yDist * yDist);
            }

            // Sort the sprites list by the sprites distance,
            // using some dark linq magic.
            sprites = sprites
                .Select((value, index) => new { Index = index, Value = value })
                .OrderBy(o => spriteDist[o.Index])
                .Select(o => o.Value)
                .Reverse()
                .ToList();

            for (int i = 0; i < sprites.Count; i++)
            {

                // Grab a reference to the current sprite, since it'll be used a
                // lot, and this should hopefully make it slightly faster and
                // more understandable.
                Sprite curSpr = sprites[i];

                // Don't render the sprite, if it isn't supposed to be rendered.
                if (!curSpr.doRender)
                    continue;

                // Grab a reference of the current sprites texture.
                Texture sprTex = curSpr.GetTexture();
                // The relative sprite position from the camera.
                Vector2d relSprPos = curSpr.getPos() - pos;

                // The imaginary camera matrix, which will be used for the
                // transformations
                Matrix2x2d camMat = new Matrix2x2d(new double[] {plane.x, dir.x,
                                                                 plane.y, dir.y});

                // The position of the sprite, transformed by the inverse of the
                // imaginary camera matrix. The camera matrix holds the position
                // and rotation of the camera, so by multiplying the sprite pos
                // by it, the camera stays still, and the sprite moves in the
                // opposite direction.
                // The x-coordinate is the 1-dimensional x-position of the
                // transformed sprite. The y-coordinate is the distance from the
                // camera.
                Vector2d transformed = camMat.getInverse() * relSprPos;

                // Cull sprites, that are behind the camera. Since the
                // distance variable isn't absolute, so a negative value,
                // means that it is behind the camera.
                if (transformed.y < 0)
                    continue;

                // The screen-space middle x-position of the sprite.
                int spriteScreenX = (int)((game.Width / 2) * (1 + transformed.x / transformed.y));

                // The screen-space height of the given sprite. This is
                // calculated, by dividing the height of the screen, by the
                // camera distance to the sprite. This makes sense, since when
                // the distance to the sprite goes down, we want it to be
                // larger. To do this, we would use the reciprocal of the
                // distance. To then scale it up, we multiply by the screen
                // height. This gives the following equation:
                //
                //    1
                // -------- * height
                // distance
                //
                // This equation is then transformed to:
                //
                //  1 * height     height
                // ----------- =  --------
                //   distance     distance
                //
                // This is called spriteScreenSize, as we assume the width to be
                // the same as the height.
                int spriteScreenSize = (int)((game.Height / transformed.y));

                // The screenspace x-positionm, at which to start drawing the
                // sprite. This is calculated, by taking away half of the sprite
                // width, from the middle of the drawing pos.
                //
                // This is capped, to not go belov zero, since that would be
                // outside the screen. It might seem, like this would draw
                // off-camera sprites, but they were culled earlier.
                int startX = spriteScreenX - spriteScreenSize / 2;
                startX = Math.Max(0, startX);
                // Same as startX, excepts it adds half the width and caps at
                // screen width, instead.
                int endX = spriteScreenSize / 2 + spriteScreenX;
                endX = Math.Min(endX, game.Width);

                // Calculates the darkening of the sprite, based of the distance
                // to the camera.
                float darken = 0.9f;
                darken = (float)Math.Min(1, Math.Max(0, darken - transformed.y * (visRange * 0.005)));

                // Goes through all columns, from statX to endX.
                for (int x = startX; x < endX; x++)
                {
                    // The x-coordnate on the sprite texture, corresponding to the
                    int texX = (int)(256 * (x - (-spriteScreenSize / 2 + spriteScreenX)) * sprTex.width / spriteScreenSize) / 256;

                    // Cull columns, that are behind walls, by looking at
                    // the wall-zbuffer and comparing it to the distance.
                    // The zbuffer doesn't hold the z-positions of the
                    // sprites, since sprites are the only thing using it,
                    // and they were sorted earlier, thereby using the
                    // painters algorihm.

                    if (transformed.y < ZBuffer[x])
                    {
                        curSpr.UpdateOnDraw(transformed.y);
                        TexColor mixedLight = new TexColor(255, 255, 255);
                        Vector2d newPlane = ((plane * -1) + ((plane * 2)) / (endX - startX) * (x - startX));

                        LightDist[] lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, curSpr.pos + newPlane);
                        if (lights.Count() < 0)
                            mixedLight = LightDistHelpers.MixLightDist(lightDists);

                        // if(World.dayTime > 0.5f) {
                        //     darken *= 0.6f;
                        // } else {
                        //     Vector2d realPosAbove = new Vector2d(curSpr.pos.x, curSpr.pos.y);

                        //     const float offset = 20;
                        //     realPosAbove.x += 0.1;
                        //     realPosAbove.y += World.dayTime * offset - offset/4;
                        //     Vector2i cellPosAbove = (Vector2i)realPosAbove;
                        //     if(map.GetRoof(cellPosAbove.x, cellPosAbove.y) != 0 || map.IsWall(cellPosAbove.x, cellPosAbove.y))
                        //         darken *= 0.6f;
                        // }

                        game.DrawVerLine(x, spriteScreenSize, sprTex, texX, darken, mixedLight, map.lightMix, new TexColor(0, 0, 0));
                    }
                }
            }
        }
    }
}