using System;
using System.Diagnostics;
using textured_raycast.maze.math;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.graphics
{
    class WallCasting
    {
        public static void WallCast(ref ConsoleBuffer game, ref double[] ZBuffer, float visRange, Map map = null)
        {
	    if(map is null)
                map = World.curMap;
            // map = World.getMapByID(World.currentMap);

            int width = game.Width;
            int height = game.Height;
            ILight[] lights = map.GetLights();

            // Loop through every x in the "window", casting a ray for each.
            // ---
            // Raycasting is done using the digital differential analyzer
            // algorithm. For a straight line, the x distance, between
            // intersections of a grid on the y axis, is the same. Same goes
            // inverse. By checking all the intersected cells for both,
            // swtiching between them and always using the current shortest,
            // the first gridcell intersection can be found.
            WallcastReturn[] casted = new WallcastReturn[width];


            // Parallel.For(0, game.Width,
            //              x => {
            //                  casted[x] = DoOneWallcast(x, width, height, lights, dir, plane, pos, visRange, map);
            //                  });

            for (int x = 0; x < game.Width; x++) {
                casted[x] = DoOneWallcast(x, width, height, lights, World.plrRot, World.plrPos, visRange, 0, 0, map);
                WallcastReturn cast = casted[x];
                // Draw the ray.
                if (cast.HitWall.doDraw) {
                    game.DrawVerLine(x, cast.LineHeight, cast.Tex, cast.TexX, cast.Darken, cast.MixedLight, map.lightMix, null);
                }

                // Set z-buffer
                ZBuffer[x] = cast.PerpWallDist;
            }
        }

        public struct WallcastReturn {
            public int LineHeight;
            public Texture Tex;
            public int TexX;
            public float Darken;
            public double PerpWallDist;
            public TexColor MixedLight;
            public Wall HitWall;

            public WallcastReturn(int LineHeight, Texture Tex, int TexX, float Darken, double PerpWallDist, TexColor MixedLight, Wall hitWall) {
                this.LineHeight = LineHeight;
                this.Tex = Tex;
                this.TexX = TexX;
                this.Darken = Darken;
                this.PerpWallDist = PerpWallDist;
                this.MixedLight = MixedLight;
                this.HitWall = hitWall;
            }
        }

        public static WallcastReturn DoOneWallcast(int x, int width, int height, ILight[] lights, Vector2d dir, Vector2d pos, float visRange, double alreadyDist = 0, int recurseCount = 0, Map map = null) {

	    if(map is null)
		map = World.getMapByID(World.currentMap);

            // The current x-coordinate on the camera viewport "plane"
            // (line), corresponding to the current viewspace
            // x-coordinate.
            // The x-range of the rendering space then becomes [-1;1].
            // This allows for easier further calculations.
            //
            // Formula:
            // (   2*x   )
            // ( ------- ) - 1
            // (  Width  )
            double cameraX = ((2 * x) / (double)width) - 1;
            // A unit vector, representing the direction of the
            // currently cast ray. Calculated by the player direction
            // plus part of the viewport "plane".
            Vector2d rayDir = dir + (World.plrPlane * cameraX);

            // The player position in map-coordinates.
            Vector2i mapPos = (Vector2i)pos.Floor();

            // sideDist holds the initial length, needed to travel, for
            // the ray to be on an x-intersection and a y-intersection.
            Vector2d sideDist = new Vector2d(0, 0);
            // The x-value holds the amount, x has to increase by, to go
            // from one intersection of the grid in the y-axis, to
            // another. The y-value is the opposite.
            // The ternary operator is used to avoid division by zero,
            // setting it to a really high number in that case.
            Vector2d diffDist = new Vector2d(rayDir.x == 0 ? 100000000 : Math.Abs(1 / rayDir.x),
                                             rayDir.y == 0 ? 100000000 : Math.Abs(1 / rayDir.y));
            // The distance to the intersected cell, perpendicular to
            // the camera plane.
            double perpWallDist;

            // Holds the direction to move in for x and y.
            // X: -1 = left ; +1 = right
            // Y: -1 = up   ; +1 = down
            Vector2i step = new Vector2i(0, 0);

            // Sets step variable and calculates sideDist for both x and y.
            if (rayDir.x < 0)
            {
                step.x = -1;
                sideDist.x = (pos.x - (double)mapPos.x) * diffDist.x;
            }
            else
            {
                step.x = 1;
                sideDist.x = ((double)mapPos.x + 1 - pos.x) * diffDist.x;
            }

            if (rayDir.y < 0)
            {
                step.y = -1;
                sideDist.y = (pos.y - (double)mapPos.y) * diffDist.y;
            }
            else
            {
                step.y = 1;
                sideDist.y = ((double)mapPos.y + 1 - pos.y) * diffDist.y;
            }

            // Whether or not the ray hit a wall. Used to get out of a
            // while loop.
            bool hit = false;
            // The cell-type / number of the hit wall.
            Wall hitWall = null;
            // Whether it was hit in a y-intersection or an
            // x-intersection.
            int side = 0;
            // This while loop runs, until a ray hits a cell.
            while (!hit)
            {
                // DDA essentially just casts two rays the same direction.
                // On looking for x-intersections and one for
                // y-intersections. We switch between the two, depending
                // on which is currently shorter, then keep casting
                // that, until the other is shorter.
                // SideDist is now holding the full ray distance.
                if (sideDist.x < sideDist.y)
                {
                    // Increment sideDist by the distance between
                    // intersections.
                    sideDist.x += diffDist.x;
                    // mapPos now holds the position of the intersected
                    // cell.
                    mapPos.x += step.x;
                    // Set side, since we know, if this ray hit, the
                    // side was 0.
                    side = 0;
                }
                else
                {
                    // Increment sideDist by the distance between
                    // intersections.
                    sideDist.y += diffDist.y;
                    // mapPos now holds the position of the intersected
                    // cell.
                    mapPos.y += step.y;
                    // Set side, since we know, if this ray hit, the
                    // side was 0.
                    side = 1;
                }

                // Check if the currently intersected cell was not
                // empty.
                if (map.IsWall((int)mapPos.x, (int)mapPos.y))
                {
                    hit = true;
                    hitWall = map.GetWall((int)mapPos.x, (int)mapPos.y);
                }

            }

            // Calculate the distance to the wall, depending on which
            // intersection was made. This is because DDA essentially
            // just casts two rays the same direction. On looking for
            // x-intersections and one for y-intersections. We use the
            // side variable to know which ray hit, calculating the
            // distance it traveled.
            if (side == 0)
                perpWallDist = (sideDist.x - diffDist.x);
            else
                perpWallDist = (sideDist.y - diffDist.y);

            // lineHeight stores the height needed to draw the ray in
            // screen coordinates.
            // It is calculated in a try-catch block, to catch a
            // division by zero and in that case, make it a very large
            // number.
            int lineHeight;
            try
            {
                lineHeight = Convert.ToInt32(height / (perpWallDist + alreadyDist));
            }
            catch (Exception)
            {
                lineHeight = 1000;
            }

            // Darken color based on distance and visRange variable.
            float darken = 0.9f;
            darken = (float)Math.Max(0, darken - perpWallDist * (visRange * 0.005));

            Texture tex = ResourceManager.getTexture(hitWall == null ? "" : hitWall.textPath);

            double wallX;
            if (side == 0)
                wallX = pos.y + perpWallDist * rayDir.y;
            else
                wallX = pos.x + perpWallDist * rayDir.x;
            wallX -= (int)wallX;

            int texX = (int)(wallX * tex.width);
            // if(side == 0 && rayDir.x > 0) texX = tex.width - texX - 1;
            // if(side == 1 && rayDir.y < 0) texX = tex.width - texX - 1;

            // Calculate the map position of the ray hit.
            Vector2d hitPos = new Vector2d(
                pos.x + perpWallDist * rayDir.x,
                pos.y + perpWallDist * rayDir.y
            );

            if(World.dayTime > 0.5f) {
                darken *= 0.6f;
            } else {
                Vector2d realPosAbove = new Vector2d(hitPos.x + 0.1, hitPos.y);
                Vector2i cellPosAbove = (Vector2i)realPosAbove;
                if(map.GetRoof(cellPosAbove.x, cellPosAbove.y) != "" || map.IsWall(cellPosAbove.x, cellPosAbove.y))
                    darken *= 0.6f;
            }

            /*
            if(hitWall.wallID == 5 && recurseCount < 5) {
                Vector2d newDir;
                if(side == 0)
                    newDir = new Vector2d(-dir.x, dir.y);
                else
                    newDir = new Vector2d(dir.x, -dir.y);
                return DoOneWallcast(x, width, height, lights, newDir, hitPos, visRange, perpWallDist + alreadyDist, recurseCount+1);
            }
            */// removed beacuse it wont be used <3, and we dont use ids for walls no more...


            // Do Lighting
            // ===========

            LightDist[] lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, hitPos);
            TexColor mixedLight = new TexColor(255, 255, 255);
            // if(lights.Count() <= 0) {
                mixedLight = LightDistHelpers.MixLightDist(lightDists);
            // }


            return new WallcastReturn(lineHeight, tex, texX, darken, perpWallDist + alreadyDist, mixedLight, hitWall);
        }
    }
}
