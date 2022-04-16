using System;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.graphics
{
    class FloorCasting
    {
        public static void FloorCast(ref Texture game, float visRange, Map map = null)
        {
	    if(map is null)
		map = World.getMapByID(World.currentMap);

            game.Clear();

            ILight[] lights = map.GetLights();

            // Grabs the floor and ceiling texture, before the loop, since we
            // don't want differently textured ceiling or floor.

            Texture floorTex   = ResourceManager.getTexture(World.textures[map.floorTexID]);
	    Texture ceilingTex = ResourceManager.getTexture(World.textures[map.useSkybox ? 1 : map.ceilTexID]);

            // Grab the windiw dimensions, since they'll be used a lot.
            int winWidth  = game.width;
            int winHeight = game.height;

            LightDist[] lightDists;
            TexColor mixedLight = new TexColor(0, 0, 0);
            // Loop through every row in the window.
            for(int y = winHeight/2; y < winHeight; y++)
            {
                // Calculatethe direction vector, for a vector going from the
                // player position, through the imaginary cameraplane, on both
                // sides of said plane.
                Vector2d rayDirLeft = World.plrRot - World.plrPlane;
                Vector2d rayDirRight = World.plrRot + World.plrPlane;

                // Calculate the current rows offset from the middle of the
                // screen.
                int midOff = y - (winHeight / 2)+1;
                // Get the camera height, assuming it to be in the middle of the
                // screen.
                float camHeight = 0.5f * winHeight - World.plrBob * 2;
                float lineDist = camHeight / midOff;
                // Cap lineDist, since it'll be casted to an int later.
                lineDist = lineDist < 1000000000 ? lineDist : 1000000000;

                Vector2d floorOff = lineDist * (rayDirRight - rayDirLeft) / winWidth;

                Vector2d floor = World.plrPos + (new Vector2d(lineDist, lineDist) * rayDirLeft);

                for(int x = 0; x < winWidth; x++) {
                    if(lights.Count() > 0) {
                        lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, floor);
                        mixedLight = LightDistHelpers.MixLightDist(lightDists);
                    }

                    Vector2i cellPos = (Vector2i)floor.Floor();
                    int floorId = map.GetFloor(cellPos.x, cellPos.y);
                    floorTex = floorId == 0 ? null : ResourceManager.getTexture(World.textures[floorId]);

                    int ceilId = map.GetRoof(cellPos.x, cellPos.y);
                    ceilingTex = ceilId == 0 ? null : ResourceManager.getTexture(World.textures[ceilId]);

                    float darken = 0.9f;
                    if (!map.useSkybox)
                        darken = 1f;
                    
                    darken = (float)Math.Min(1, Math.Max(0, darken - lineDist * (visRange * 0.005)));

                    TexColor texColor = new TexColor(0, 0, 0);
                    TexColor color = new TexColor(0, 0, 0);


                    // Floor Code
                    // ==========
                    if(!(floorTex is null)) {
                        Vector2i texture = (Vector2i)(floorTex.width * (floor - (Vector2d)cellPos)).Floor();
                        texture = new Vector2i(
                            Math.Abs(texture.x),
                            Math.Abs(texture.y)
                        );

                        texColor = floorTex.getPixel(texture.x, texture.y);
                        color  = texColor * darken * map.lightMix;
                        color += maze.texture.TexColor.unitMultReal(texColor, mixedLight) * (1 - map.lightMix);
                        if(World.dayTime > 0.5f) {
                            color *= 0.6f;
                        } else {
                            Vector2d realPosAbove = new Vector2d(floor.x + 0.1, floor.y);
                            Vector2i cellPosAbove = (Vector2i)realPosAbove;
                            if(map.GetRoof(cellPosAbove.x, cellPosAbove.y) != 0 || map.IsWall(cellPosAbove.x, cellPosAbove.y))
                                color *= 0.6f;
                        }
                        game.setPixel(x, y, color);
                    }

                    // Ceiling code
                    // ============
                    if(ceilingTex is null) {
                        var pix = Skybox.GetSkyboxPixel(winHeight, ResourceManager.getTexture(World.textures[99]), x, winHeight - y - 1, World.dayTime);
                        if((game.getPixel(x, winHeight-y-1) is null))
                            game.setPixel(x, winHeight-y-1, pix);
                    } else {
                        Vector2i texture = (Vector2i)(ceilingTex.width * (floor - (Vector2d)cellPos)).Floor();
                        texture = new Vector2i(
                            Math.Abs(texture.x),
                            Math.Abs(texture.y)
                        );

                        texColor = ceilingTex.getPixel(texture.x, texture.y);
                        color  = texColor * darken;
                        color *= map.lightMix * 0.6f;
                        color += maze.texture.TexColor.unitMultReal(texColor, mixedLight) * (1-map.lightMix);

                        int shownLines = Math.Max(0, Math.Min((int)((float)15 / (lineDist)), 5));
                        for (int i = 1; i <= shownLines; i++)
                        {
                            game.setPixel(x, winHeight - y - i - (int)(World.plrBob * 2), color * (1.0f - (0.2f * (i-1))));
                        }
                    }

                    floor += floorOff;
                }
            }
        }
    }
}
