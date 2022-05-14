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
                map = World.curMap;
            // map = World.getMapByID(World.currentMap);

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

		// Get the offset, to change the floor variable by at
		// each iteration of the following loop
                Vector2d floorOff = lineDist * (rayDirRight - rayDirLeft) / winWidth;

		// Get the world position of the floor at the current
		// y-coordinate and the leftmost x on the screen.
                Vector2d floor = World.plrPos + (new Vector2d(lineDist, lineDist) * rayDirLeft);

		// Run through each x-pixel on the screen.
                for(int x = 0; x < winWidth; x++) {
		    // If there are lights in the world, calculate them.
                    if(lights.Count() > 0 || World.player.HoldsLight) {
                        lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, floor);
                        mixedLight = LightDistHelpers.MixLightDist(lightDists);
                    }

		    // Get the floor position in the grid, by flooring
		    // it. Then get the texture of that floor and save
		    // it in the floorTex variable.
                    Vector2i cellPos = (Vector2i)floor.Floor();
                    string floorId = map.GetFloor(cellPos.X, cellPos.Y);
                    floorTex = floorId == "" ? null : ResourceManager.getTexture(floorId);

		    // Get and save roof texture too.
                    string ceilId = map.GetRoof(cellPos.X, cellPos.Y);
                    ceilingTex = ceilId == "" ? null : ResourceManager.getTexture(ceilId);

		    // Set and calculate distance-based darkening,
		    // depending on whether the current map uses the
		    // skybox and therefore has an open roof.
                    float darken = 0.9f;
                    if (!map.useSkybox)
                        darken = 1f;
                    darken = (float)Math.Min(1, Math.Max(0, darken - lineDist * (visRange * 0.005)));

                    TexColor texColor = new TexColor(0, 0, 0);
                    TexColor color = new TexColor(0, 0, 0);

                    // Floor Code
                    // ==========
                    if(!(floorTex is null)) {

			// Gets the position on the current texture,
			// at which it should be drawn.
                        Vector2i TexturePosition = (Vector2i)(floorTex.width * (floor - (Vector2d)cellPos)).Floor();
                        TexturePosition = new Vector2i(
                            Math.Abs(TexturePosition.X),
                            Math.Abs(TexturePosition.Y)
                        );
			
			// Gets the current pixel and saves it in the
			// texColor variable.
                        texColor = floorTex.getPixel(TexturePosition.X, TexturePosition.Y);

			// Add distance-based darkening to texColor
			// and save it in color.
                        color  = texColor * darken * map.lightMix;

			// Add lights to the color variable.
                        color += maze.texture.TexColor.unitMultReal(texColor, mixedLight) * (1 - map.lightMix);

			// If it's night, darken the color by
			// multiplying with 0.6. Else, darken if in
			// shadow.
                        if(World.dayTime > 0.5f) {
                            color *= 0.6f;
                        } else {
			    // Take the current position and move its
			    // x-coordinate by a slight amount, to
			    // simulate the sun coming from a
			    // direction. The get the position of the
			    // in the grid, by converting it to a
			    // Vector2i.
                            Vector2d realPosAbove = new Vector2d(floor.X + 0.1, floor.Y);
                            Vector2i cellPosAbove = (Vector2i)realPosAbove;

			    // Check, whether or not there is a roof
			    // or a wall at the moved position. If
			    // there is, darken the floor.
                            if(map.GetRoof(cellPosAbove.X, cellPosAbove.Y) != "" || map.IsWall(cellPosAbove.X, cellPosAbove.Y))
                                color *= 0.6f;
                        }
			// Draw the pixel to the screen.
                        game.setPixel(x, y, color);
                    }

                    // Ceiling code
                    // ============
                    if(ceilingTex is null) {
			// If there is no ceiling at the current
			// position, it must be a skybox, so draw
			// that.
                        var pix = Skybox.GetSkyboxPixel(winHeight, ResourceManager.getTexture(World.textures[99]), x, winHeight - y - 1, World.dayTime);
                        if((game.getPixel(x, winHeight-y-1) is null))
                            game.setPixel(x, winHeight-y-1, pix);
                    } else {

			// Gets the position on the current texture,
			// at which it should be drawn.
                        Vector2i texture = (Vector2i)(ceilingTex.width * (floor - (Vector2d)cellPos)).Floor();
                        texture = new Vector2i(
                            Math.Abs(texture.X),
                            Math.Abs(texture.Y)
                        );

			// Gets the current pixel and saves it in the
			// texColor variable.
                        texColor = ceilingTex.getPixel(texture.X, texture.Y);

			// Add distance-based darkening to texColor
			// and save it in color.
                        color  = texColor * darken * map.lightMix;

			// Add lights to the color variable.
                        color *= map.lightMix * 0.6f;

			// Add lights to the color variable.
                        color += maze.texture.TexColor.unitMultReal(texColor, mixedLight) * (1-map.lightMix);
			
			// ShownLines holds the calculated thickness
			// of the roof, which should get smaller, as
			// the player gets further from it.
                        int shownLines = Math.Max(0, Math.Min((int)((float)15 / (lineDist)), 5));

			// The roof is drawn ShownLines amount of
			// times, moving the y-coordinate of the pixel
			// by one each iteration.
                        for (int i = 1; i <= shownLines; i++)
                        {
                            game.setPixel(x, winHeight - y - i - (int)(World.plrBob * 2), color * (1.0f - (0.2f * (i-1))));
                        }
                    }

		    // Floor is moved by the flooroffset.
                    floor += floorOff;
                }
            }
        }
    }
}
