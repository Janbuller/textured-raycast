using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Drawing;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using Pastel;

namespace textured_raycast.maze
{
    class Maze {

        // Numbers and their colors.
        static Dictionary<int, Color> colors = new Dictionary<int, Color>() {
            {1, Color.FromArgb(255, 255, 255)},
            {2, Color.FromArgb(0,   255, 0  )},
            {3, Color.FromArgb(255, 0,   0  )},
            {101, Color.FromArgb(0,   255, 0  )}, // Also used as collision box for winning.
            {102, Color.FromArgb(255, 0,   0  )}, // Also used for leaving the maze
        };

        static Dictionary<int, Texture> textures = new Dictionary<int, Texture>() {
            {1,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/greystone.ppm")},
            {2,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/redbrick.ppm")},
            {3,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/bluestone.ppm")},
            {4,   TextureLoaders.loadFromPlainPPM("img/test5.ppm")},
            {5,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/redstone.ppm")},
            {6,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/colorstone.ppm")},
            {7,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/pillar.ppm")},
            {8,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/barrel.ppm")},
            {9,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/greenlight.ppm")},
            {10,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/barrelBroken.ppm")},
            {11,   TextureLoaders.loadFromPlainPPM("img/skybox.ppm")},
            {101, TextureLoaders.loadFromPlainPPM("img/wolfenstein/end.ppm")}, // Also used as collision box for winning.
            {102, TextureLoaders.loadFromPlainPPM("img/wolfenstein/exit.ppm")}, // Also used for leaving the maze
        };

        // Returns true if maze is completed, false if exited.
        public static bool StartMaze(World world) {
            return Start(world);
        }

        private static bool Start(World world) {

            Map map = world.getMapByID(world.currentMap);

            Console.Clear();
            MazeEngine game = new MazeEngine(120, 80, "maze");

            // Position vector
            Vector2d pos = world.plrPos;

            // The distance of witch the player can interact
            double interactDist = 0.4;

            // Directional unit vector
            Vector2d dir = world.plrRot;

            // Camera view plane, held as 2d vector line.
            // Were this actually 3d, not raycasting, it would be a plane,
            // represtented by 2 vectors.
            Vector2d plane = new Vector2d(0.66f, 0);

            // The visibility distance. Controls the distance-based darkening.
            int visRange = 25;

            double[] ZBuffer = new double[game.GetWinWidth()];

            // Main game loop
            while(true)
            {
                // make sure it knows what map its on
                map = world.getMapByID(world.currentMap);

                pos = world.plrPos;
                dir = world.plrRot;

                //DrawSkybox(ref game, dir, textures[1]);

                // Do the floor/ceiling casting.
                FloorCasting(ref game, dir, plane, pos, visRange, map);

                // Loop through every x in the "window", casting a ray for each.
                // ---
                // Raycasting is done using the digital differential analyzer
                // algorithm. For a straight line, the x distance, between
                // intersections of a grid on the y axis, is the same. Same goes
                // inverse. By checking all the intersected cells for both,
                // swtiching between them and always using the current shortest,
                // the first gridcell intersection can be found.
                for(int x = 0; x < game.GetWinWidth(); x++) {
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
                    double cameraX = ( (2 * x) / (double)game.GetWinWidth()) - 1;
                    // A unit vector, representing the direction of the
                    // currently cast ray. Calculated by the player direction
                    // plus part of the viewport "plane".
                    Vector2d rayDir = dir + (plane * cameraX);

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
                    if(rayDir.x < 0) {
                        step.x = -1;
                        sideDist.x = (pos.x - (double)mapPos.x) * diffDist.x;
                    } else {
                        step.x = 1;
                        sideDist.x = ((double)mapPos.x + 1 - pos.x) * diffDist.x;
                    }

                    if(rayDir.y < 0) {
                        step.y = -1;
                        sideDist.y = (pos.y - (double)mapPos.y) * diffDist.y;
                    } else {
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
                    while(!hit) {
                        // DDA essentially just casts two rays the same direction.
                        // On looking for x-intersections and one for
                        // y-intersections. We switch between the two, depending
                        // on which is currently shorter, then keep casting
                        // that, until the other is shorter.
                        // SideDist is now holding the full ray distance.
                        if(sideDist.x < sideDist.y) {
                            // Increment sideDist by the distance between
                            // intersections.
                            sideDist.x += diffDist.x;
                            // mapPos now holds the position of the intersected
                            // cell.
                            mapPos.x += step.x;
                            // Set side, since we know, if this ray hit, the
                            // side was 0.
                            side = 0;
                        } else {
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
                        if(map.IsWal((int)mapPos.x, (int)mapPos.y)) {
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
                    if(side == 0)
                        perpWallDist = (sideDist.x - diffDist.x);
                    else
                        perpWallDist = (sideDist.y - diffDist.y);

                    // lineHeight stores the height needed to draw the ray in
                    // screen coordinates.
                    // It is calculated in a try-catch block, to catch a
                    // division by zero and in that case, make it a very large
                    // number.
                    int lineHeight;
                    try {
                        lineHeight = Convert.ToInt32(game.GetWinHeight() / perpWallDist);
                    } catch (Exception e) {
                        lineHeight = 1000;
                    }

                    float darken = 1;
                    // Actually draw the raycast line. Darken color depending on
                    // facing side, simulating lighting,
                    if(side == 0) {
                        // Construct darker color
                        darken = 1; // B - i removed this because it looked dumb knowing that there was no lighting in the room (and it made distance shadows unequal) 
                    }

                    // Darken color based on distance and visRange variable.
                    darken = (float)Math.Max(0, darken - perpWallDist * (visRange * 0.005));

                    Texture tex = textures[hitWall == null ? 1 : hitWall.thisTexID];
                    double wallX;
                    if (side == 0)
                        wallX = pos.y + perpWallDist * rayDir.y;
                    else
                        wallX = pos.x + perpWallDist * rayDir.x;
                    wallX -= (int)wallX;

                    int texX = (int)(wallX * tex.width);
                    // if(side == 0 && rayDir.x > 0) texX = tex.width - texX - 1;
                    // if(side == 1 && rayDir.y < 0) texX = tex.width - texX - 1;

                    // Draw the ray.
                    if (hitWall.doDraw)
                        game.DrawVerLine(x, lineHeight, tex, texX, darken);

                    // Set z-buffer
                    ZBuffer[x] = perpWallDist;
                }

                SpriteCasting(ref game, map.sprites, pos, plane, dir, ZBuffer, visRange);

                // not really neccecary
                //game.DrawBorder();

                // Add textbox to draw if neccecary

                Sprite spriteToInteract = null;
                double distanceToInteract = 9999;

                foreach (Sprite sprite in map.sprites)
                {
                    double distance = pos.DistTo(sprite.getPos());
                    if (distance < interactDist && distance < distanceToInteract && sprite.canInteract)
                    {
                        spriteToInteract = sprite;
                        distanceToInteract = distance;
                    }
                }

                if (spriteToInteract != null)
                    world.interactMessage = spriteToInteract.ActivateMessage();
                else
                    world.interactMessage = "";

                Console.Write(world.currentMessage == "" ? world.interactMessage : world.currentMessage);

                Console.WriteLine("                                                                  ");

                game.DrawTexture(textures[8], -8, -24, new TexColor(0, 0, 0));

                game.SwapBuffers();
                game.DrawScreen();

                // Handle movement
                double rotSpeed = 0.2;
                double movSpeed = 0.1;
                while (Console.KeyAvailable) {
                    world.currentMessage = "";
                    // Reads and saves pressed key
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    // Checks the pressed key. Sends press to menu.

                    // Multiplied with movement speed, during collision check,
                    // forcing the player to stay slightly further away from
                    // walls.
                    float extraColDistMult = 1f;
                    if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                    {
                        // CellX and CellY holds the cell, the player would move
                        // into, in those directions. Using a vector doesn't
                        // make sense, since they could be different. They are
                        // split up, to allow sliding on walls, when not walking
                        // perpendicular into them.
                        Wall cellX = map.GetWall((int)(pos.x + dir.x * (movSpeed * extraColDistMult)), (int)(pos.y));
                        Wall cellY = map.GetWall((int)(pos.x), (int)(pos.y + dir.y * (movSpeed * extraColDistMult)));

                        // Check if cell is empty or a control cell, if so, move.
                        if (!cellX.isWal) pos.x += dir.x * movSpeed;
                        if (!cellY.isWal) pos.y += dir.y * 0.1;

                        cellX.Collide(ref world);
                        cellY.Collide(ref world);
                    }
                    else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                    {
                        // Same as before, just backwards, so with subtraction instead of addition.
                        Wall cellX = map.GetWall((int)(pos.x - dir.x * (movSpeed * extraColDistMult)), (int)(pos.y));
                        Wall cellY = map.GetWall((int)(pos.x), (int)(pos.y - dir.y * (movSpeed * extraColDistMult)));
                        if (!cellX.isWal) pos.x -= dir.x * movSpeed;
                        if (!cellY.isWal) pos.y -= dir.y * 0.1;

                        cellX.Collide(ref world);
                        cellY.Collide(ref world);
                    }
                    else if (key.Key == ConsoleKey.D)
                    {
                        Vector2d newDir = new Vector2d(-dir.y, dir.x);

                        // Same as before, just backwards, so with subtraction instead of addition.
                        Wall cellX = map.GetWall((int)(pos.x - newDir.x * (movSpeed * extraColDistMult)), (int)(pos.y));
                        Wall cellY = map.GetWall((int)(pos.x), (int)(pos.y - newDir.y * (movSpeed * extraColDistMult)));
                        if (!cellX.isWal) pos.x -= newDir.x * movSpeed;
                        if (!cellY.isWal) pos.y -= newDir.y * 0.1;

                        cellX.Collide(ref world);
                        cellY.Collide(ref world);
                    }
                    else if (key.Key == ConsoleKey.A)
                    {
                        Vector2d newDir = new Vector2d(-dir.y, dir.x) * -1;

                        // Same as before, just backwards, so with subtraction instead of addition.
                        Wall cellX = map.GetWall((int)(pos.x - newDir.x * (movSpeed * extraColDistMult)), (int)(pos.y));
                        Wall cellY = map.GetWall((int)(pos.x), (int)(pos.y - newDir.y * (movSpeed * extraColDistMult)));
                        if (!cellX.isWal) pos.x -= newDir.x * movSpeed;
                        if (!cellY.isWal) pos.y -= newDir.y * 0.1;

                        cellX.Collide(ref world);
                        cellY.Collide(ref world);
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        // Use too much math, to calculate the direction unit vector.
                        double oldDirX = dir.x;
                        dir.x = dir.x * Math.Cos(-rotSpeed) - dir.y * Math.Sin(-rotSpeed);
                        dir.y = oldDirX * Math.Sin(-rotSpeed) + dir.y * Math.Cos(-rotSpeed);
                        // Use too much math, to calculate the camera viewport plane.
                        double oldPlaneX = plane.x;
                        plane.x = plane.x * Math.Cos(-rotSpeed) - plane.y * Math.Sin(-rotSpeed);
                        plane.y = oldPlaneX * Math.Sin(-rotSpeed) + plane.y * Math.Cos(-rotSpeed);
                    }
                    else if (key.Key == ConsoleKey.LeftArrow)
                    {
                        // Use too much math, to calculate the direction unit vector.
                        double oldDirX = dir.x;
                        dir.x = dir.x * Math.Cos(rotSpeed) - dir.y * Math.Sin(rotSpeed);
                        dir.y = oldDirX * Math.Sin(rotSpeed) + dir.y * Math.Cos(rotSpeed);
                        // Use too much math, to calculate the camera viewport plane.
                        double oldPlaneX = plane.x;
                        plane.x = plane.x * Math.Cos(rotSpeed) - plane.y * Math.Sin(rotSpeed);
                        plane.y = oldPlaneX * Math.Sin(rotSpeed) + plane.y * Math.Cos(rotSpeed);
                    }
                    else if (key.Key == ConsoleKey.E)
                    {
                        if (spriteToInteract != null)
                            spriteToInteract.Activate(ref world);
                    }
                };
            }
        }

        public static void FloorCasting(ref MazeEngine game, Vector2d dir, Vector2d plane, Vector2d pos, float visRange, Map map) {
            Texture floorTex =  textures[map.floorTexID];
            Texture ceilingTex =  textures[map.useSkybox ? 1 : map.ceilTexID];
            for(int y = 0; y < game.GetWinHeight(); y++)
            {
                Vector2d rayDirLeft = dir - plane;
                Vector2d rayDirRight = dir + plane;

                int midOff = y - game.GetWinHeight() / 2;
                float camHeight = 0.5f * game.GetWinHeight();
                float lineDist = camHeight / midOff;
                lineDist = lineDist < 1000000000 ? lineDist : 1000000000;

                Vector2d floorOff = lineDist * (rayDirRight - rayDirLeft) / game.GetWinWidth();

                Vector2d floor = pos + (new Vector2d(lineDist, lineDist) * rayDirLeft);

                for(int x = 0; x < game.GetWinWidth(); x++) {
                    Vector2i cellPos = (Vector2i)floor.Floor();
                    Vector2i texture = (Vector2i)(floorTex.width * (floor - (Vector2d)cellPos)).Floor();
                    texture = new Vector2i(
                        Math.Abs(texture.x),
                        Math.Abs(texture.y)
                    );

                    floor += floorOff;

                    float darken = 0.9f;
                    darken = (float)Math.Min(1, Math.Max(0, darken - lineDist * (visRange * 0.005)));

                    TexColor color = floorTex.getPixel(texture.x, texture.y);
                    TexColor darkPix = new TexColor(
                        Convert.ToInt32(color.r * darken),
                        Convert.ToInt32(color.g * darken),
                        Convert.ToInt32(color.b * darken)
                    );
                    if(y > game.GetWinHeight() / 2)
                        game.DrawPixel(darkPix, x, y);

                    if(!map.useSkybox) {
                        color = ceilingTex.getPixel(texture.x, texture.y);
                        darkPix = new TexColor(
                            Convert.ToInt32(color.r * darken),
                            Convert.ToInt32(color.g * darken),
                            Convert.ToInt32(color.b * darken)
                        );
                        game.DrawPixel(darkPix, x, game.GetWinHeight() - y - 1);
                    } else
                    {
                        if (y > game.GetWinHeight() / 2)
                        {
                            var pix = GetSkyboxPixel(ref game, dir, textures[11], x, game.GetWinHeight() - y - 1);
                            game.DrawPixel(pix, x, game.GetWinHeight() - y - 1);
                        }
                    }
                }
            }
        }

        public static void DrawSkybox(ref MazeEngine game, Vector2d dir, Texture skyboxTex) {
            for(int y = 0; y < game.GetWinHeight() / 2; y++) {
                for(int x = 0; x < game.GetWinWidth(); x++) {
                    var pix = GetSkyboxPixel(ref game, dir, skyboxTex, x, y);
                    game.DrawPixel(pix, x, y);
                }
            }
        }

        public static TexColor GetSkyboxPixel(ref MazeEngine game, Vector2d dir, Texture skyboxTex, int x, int y) {
            // The difference between the height of on pixel on the screen and
            // on the texture, were the texture to fill the top helf of the
            // screen.
            double heightDiff = skyboxTex.height / (game.GetWinHeight() * 0.5);
            // Calibrated pixel position. This is the pixel on the texture,
            // closest to the one on the screen at x and y.
            Vector2i cal = new Vector2i((int)(x * heightDiff), (int)(y * heightDiff));

            // Get the players rotation in radians.
            double dirRad = Math.Atan2(dir.x, dir.y);
            // The players rotation, mapped from the range [0, 2PI] to [0, 1].
            double dirMapped = dirRad / (Math.PI * 2);
            // The offset to the skybox texture, based of the mapped direction.
            int xOffset = (int)(dirMapped * skyboxTex.width);

            // Add the offset to the position.
            cal.x += xOffset;

            // While the position is outside the texture width, move back by the
            // amount over the bounds. This makes the texture repeating on the
            // x-axis.
            while(cal.x > skyboxTex.width)
                cal.x -= skyboxTex.width;
            while(cal.x < skyboxTex.width)
                cal.x += skyboxTex.width;
            // Return the pixel at the position.
            return skyboxTex.getPixel(cal.x, cal.y);
        }

        // TODO: Switch to using z-buffer, instead of painters algorithm.
        public static void SpriteCasting(ref MazeEngine game, List<Sprite> sprites, Vector2d pos, Vector2d plane, Vector2d dir, double[] ZBuffer, int visRange) {
            List<double> spriteDist = new List<double>();
            for(int i = 0; i < sprites.Count; i++) {
                // Calculate sprite distance from player, using pythagoras.
                // Since it's only used for comparing with itself, sqrt isn't required.
                double xDist = pos.x - sprites[i].getX();
                double yDist = pos.y - sprites[i].getY();

                spriteDist.Add(xDist * xDist + yDist * yDist);
            }

            // Sort the sprites list by the sprites distance,
            // using some dark linq magic.
            sprites = sprites
                .Select((value, index) => new {Index = index, Value = value})
                .OrderBy(o => spriteDist[o.Index])
                .Select(o => o.Value)
                .Reverse()
                .ToList();

            for(int i = 0; i < sprites.Count; i++) {
                // Grab a reference to the current sprite, since it'll be used a
                // lot, and this should hopefully make it slightly faster and
                // more understandable.
                Sprite curSpr = sprites[i];

                // Don't render the sprite, if it isn't supposed to be rendered.
                if(!curSpr.doRender)
                    continue;

                // Grab a reference of the current sprites texture.
                Texture sprTex = textures[curSpr.texID];
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
                if(transformed.y < 0)
                    continue;

                // The screen-space middle x-position of the sprite.
                int spriteScreenX = (int)((game.GetWinWidth() / 2) * (1 + transformed.x / transformed.y));

                // The screen-space height of the given sprite. This is
                // calculated, by dividing the height of the screen, by the
                // camera distance to the sprite. This makes sense, since when
                // the distance to the sprite goes down, we want it to be
                // larger. To do this, we would use the reciprocal of the
                // distance. To then scale it up, we multiply by the screen
                // height. This gives the following equation:
                //    1
                // -------- * height
                // distance
                //
                // This equation is then transformed to:
                //  1 * height     height
                // ----------- =  --------
                //   distance     distance
                //
                // This is called spriteScreenSize, as we assume the width to be
                // the same as the height.
                int spriteScreenSize = (int)((game.GetWinHeight() / transformed.y));

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
                endX = Math.Min(endX, game.GetWinWidth());

                // Calculates the darkening of the sprite, based of the distance
                // to the camera.
                float darken = 0.9f;
                darken = (float)Math.Min(1, Math.Max(0, darken - transformed.y * (visRange * 0.005)));

                // Goes through all columns, from statX to endX.
                for(int x = startX; x < endX; x++) {
                    // The x-coordnate on the sprite texture, corresponding to the
                    int texX = (int)(256 * (x - (-spriteScreenSize / 2 + spriteScreenX)) * sprTex.width / spriteScreenSize) / 256;

                    // Cull columns, that are behind walls, by looking at
                    // the wall-zbuffer and comparing it to the distance.
                    // The zbuffer doesn't hold the z-positions of the
                    // sprites, since sprites are the only thing using it,
                    // and they were sorted earlier, thereby using the
                    // painters algorihm.
                    if(transformed.y < ZBuffer[x])
                        game.DrawVerLine(x, spriteScreenSize, sprTex, texX, darken, new TexColor(0, 0, 0));
                }
            }
        }
    }
}
