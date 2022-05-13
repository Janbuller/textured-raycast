using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.online;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.graphics
{
    class SpriteCasting
    {
        public static void SpriteCast(ref ConsoleBuffer game, List<Sprite> spritesIn, double[] ZBuffer, int visRange, Map map)
        {
            List<Sprite> sprites = new List<Sprite>(spritesIn);
            foreach(var player in Client.players) {
                var pV = player.Value;
                if(pV.map == map.Path) {
                    sprites.Add(new PlayerSprite(pV.x, pV.y, pV.xRot, pV.yRot, new string[] {
                        "img/player/Player 1.ppm",
                        "img/player/Player 8.ppm",
                        "img/player/Player 7.ppm",
                        "img/player/Player 6.ppm",
                        "img/player/Player 5.ppm",
                        "img/player/Player 4.ppm",
                        "img/player/Player 3.ppm",
                        "img/player/Player 2.ppm",
                    }));
                }
	    }
            ILight[] lights = map.GetLights();

            List<double> spriteDist = new List<double>();
            for (int i = 0; i < sprites.Count; i++)
            {
                // Calculate sprite distance from player, using pythagoras.
                // Since it's only used for comparing with itself, sqrt isn't required.
                double xDist = World.plrPos.X - sprites[i].getX();
                double yDist = World.plrPos.Y - sprites[i].getY();

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
                Vector2d relSprPos = curSpr.getPos() - World.plrPos;

                // The imaginary camera matrix, which will be used for the
                // transformations
                Matrix2x2d camMat = new Matrix2x2d(new double[] {World.plrPlane.X, World.plrRot.X,
                                                                 World.plrPlane.Y, World.plrRot.Y});

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
                if (transformed.Y < 0)
                    continue;

                // The screen-space middle x-position of the sprite.
                int spriteScreenX = (int)((game.Width / 2) * (1 + transformed.X / transformed.Y));

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
                int spriteScreenSize = (int)((game.Height / transformed.Y));

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

                    if (transformed.Y < ZBuffer[x])
                    {
                        float darken = 1;

                        curSpr.UpdateOnDraw(transformed.Y);
                        TexColor mixedLight = new TexColor(255, 255, 255);
                        Vector2d newPlane = ((World.plrPlane * -1) + ((World.plrPlane * 2)) / (endX - startX) * (x - startX));

                        if (curSpr.effectedByLight)
                        {
                            LightDist[] lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, curSpr.pos + newPlane);
                            if (lights.Count() > 0)
                                mixedLight = LightDistHelpers.MixLightDist(lightDists);
                        }

			if(curSpr.GetType() == typeof(PlayerSprite))
                            Console.WriteLine($"{mixedLight.R}");
                        game.DrawVerLine(x, spriteScreenSize, sprTex, texX, darken, mixedLight, map.lightMix, new TexColor(0, 0, 0));
                    }
                }
            }
        }
    }
}
