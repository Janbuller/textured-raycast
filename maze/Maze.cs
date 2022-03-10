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
using System.Threading.Tasks;

namespace textured_raycast.maze
{
    class Maze {

        static Dictionary<int, Texture> textures = new Dictionary<int, Texture>() {
            {1,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/greystone.ppm")},
            {2,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/redbrick.ppm")},
            {3,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/bluestone.ppm")},
            {4,   TextureLoaders.loadFromPlainPPM("img/test5.ppm")},
            {5,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/redstone.ppm")},
            {6,   TextureLoaders.loadFromPlainPPM("img/wolfenstein/colorstone.ppm")},
            {99,  TextureLoaders.loadFromPlainPPM("img/skybox.ppm")},
            {101, TextureLoaders.loadFromPlainPPM("img/wolfenstein/end.ppm")}, // Also used as collision box for winning.
            {102, TextureLoaders.loadFromPlainPPM("img/wolfenstein/exit.ppm")}, // Also used for leaving the maze
        };

        public static bool StartMaze(World world) {
            return Start(world);
        }

        private static bool Start(World world) {

            Map map = world.getMapByID(world.currentMap);

            Console.Clear();

            Vector2i size = new Vector2i(120, 80);

            ConsoleEngine engine = new ConsoleEngine(size.x, size.y, "maze");
            ConsoleBuffer game = new ConsoleBuffer(size.x, size.y);
            ConsoleBuffer fight = new ConsoleBuffer(size.x, size.y);
            ConsoleBuffer UIHolder = new ConsoleBuffer(size.x, size.y);
            Texture FloorAndRoof = new Texture(size.x, size.y);

            // Position vector
            Vector2d pos = world.plrPos;

            // Directional unit vector
            Vector2d dir = world.plrRot;

            // Camera view plane, held as 2d vector line.
            // Were this actually 3d, not raycasting, it would be a plane,
            // represtented by 2 vectors.
            // Vector2d plane = new Vector2d(0.66f, 0);
            Vector2d plane = new Vector2d(dir.y, -dir.x) * 0.66;

            // The visibility distance. Controls the distance-based darkening.
            int visRange = 25;

            double[] ZBuffer = new double[engine.GetWinWidth()];

            Random rnd = new Random();
            // Main game loop
            while(world.state != states.Stopping)
            {
                while (world.state == states.Fighting)
                {
                    world.dt = (float)(DateTime.Now.Ticks - world.lastFrameTime) / TimeSpan.TicksPerSecond;
                    world.lastFrameTime = DateTime.Now.Ticks;

                    world.fight.tillFightBegins -= world.dt;

                    if (world.fight.tillFightBegins < 0)
                    {
                        fight.Clear();
                        world.fight.renderFightToBuffer(ref fight, ref world);

                        engine.DrawConBuffer(fight);
                        engine.SwapBuffers();
                        DrawScreen(engine);
                    }
                    else
                    {
                        UIHolder.Clear();
                        world.fight.renderFightStartScreenToBuffer(ref UIHolder, world.fight.tillFightBegins/2-0.1f);

                        engine.DrawConBuffer(game.mixBuffer(UIHolder));
                        engine.SwapBuffers();
                        DrawScreen(engine);
                    }
                }

                while (world.state == states.Paused)
                {
                    UIHolder.Clear();

                    GUI.GUI.pauseGUI(ref UIHolder, ref world);

                    engine.DrawConBuffer(game.mixBuffer(UIHolder));
                    engine.SwapBuffers();
                    DrawScreen(engine);
                }

                world.lastFrameTime = DateTime.Now.Ticks;
                while (world.state == states.Game)
                {
                    world.dt = (float)(DateTime.Now.Ticks - world.lastFrameTime)/TimeSpan.TicksPerSecond;
                    world.lastFrameTime = DateTime.Now.Ticks;

                    // Make time pass
                    world.dayTime += world.dt / 60; // 60 = 1 whole day = 60 sec
                    if (world.dayTime > 1) world.dayTime--;

                    if (InputManager.GetKey(Keys.K_SHIFT, world) == KeyState.KEY_UP){
                        world.staminaLVL = MathF.Min(world.staminaLVL + world.dt/4, 1);
                    }

                    // make sure it knows what map its on
                    map = world.getMapByID(world.currentMap);
                    visRange = map.useSkybox ? 1 : 25;

                    pos = world.plrPos;
                    dir = world.plrRot;

                    //DrawSkybox(ref game, dir, textures[1]);

                    // find closest sprite that is interactable and display interact message
                    Sprite spriteToInteract = null;
                    double distanceToInteract = 9999;

                    foreach (Sprite sprite in map.sprites)
                    {
                        sprite.Update(ref world, world.dt);
                        sprite.updateAnimation(world.dt);

                        double distance = pos.DistTo(sprite.getPos());
                        // Console.WriteLine(distance + " : " + sprite.canInteract);
                        if (distance < sprite.interactDistance && distance < distanceToInteract && sprite.canInteract)
                        {
                            if (sprite.autoInteract)
                            {
                                sprite.Activate(ref world);
                            }
                            else
                            {
                                spriteToInteract = sprite;
                                distanceToInteract = distance;
                            }
                        }
                    }

                    if (spriteToInteract != null)
                        world.interactMessage = spriteToInteract.ActivateMessage();
                    else
                        world.interactMessage = "";

                    string toSend = world.currentMessage == "" ? world.interactMessage : world.currentMessage;

                    //Clear the UI buffer
                    UIHolder.Clear();

                    // Add text-box to UI buffer if there is a string that should go in it.
                    if (toSend != "")
                    {
                        GUI.GUI.texBox(ref UIHolder, ref world, toSend);
                    }

                    // Draw the stamina bar in the UI Buffer
                    for (int i = 0; i < 78; i++)
                    {
                        if (world.staminaLVL > (float)i / 78)
                        {
                            UIHolder.DrawPixel(new TexColor(0, 155, 0), 117, 1 + i);
                            UIHolder.DrawPixel(new TexColor(0, 155, 0), 118, 1 + i);
                        }
                    }

                    // Do the floor/ceiling casting.
                    FloorCasting(ref FloorAndRoof, dir, plane, pos, visRange, map, world);
                    game.DrawTexture(FloorAndRoof, 0, 0);

                    // Do the wall casting
                    WallCasting(ref game, ref ZBuffer, dir, plane, pos, visRange, map);

                    // draw sprites
                    SpriteCasting(ref game, map.sprites, pos, plane, dir, ZBuffer, visRange, map, ref world);

                    engine.DrawConBuffer(game.mixBuffer(UIHolder));
                    engine.SwapBuffers();
                    DrawScreen(engine);
                    HandleInputGame(ref world, map, pos, ref dir, ref plane, ref spriteToInteract);
                }
            }

            return false;
        }

        // Multi-threaded screen-drawing
        // =============================

        // Whether the screen is currently being drawn
        private static bool drawing = false;
        // Draw the screen asynchronously
        public static void DrawScreen(ConsoleEngine engine) {
            if(!drawing) {

                drawing = true;

                Task.Run(() => {
                    engine.DrawScreen();
                    drawing = false;
                });

            }
        }

        public static void HandleInputGame(ref World world, Map map, Vector2d pos, ref Vector2d dir, ref Vector2d plane, ref Sprite spriteToInteract) {
            double rotSpeed = world.dt*0.8;

            // Multiplied with movement speed, during collision check,
            // forcing the player to stay slightly further away from
            // walls.

            if (InputManager.GetKey(Keys.K_UP, world) != KeyState.KEY_UP || InputManager.GetKey(Keys.K_W, world) != KeyState.KEY_UP)
            {
                moveInDir(ref world, ref map, ref pos, dir);
            }
            if (InputManager.GetKey(Keys.K_DOWN, world) != KeyState.KEY_UP || InputManager.GetKey(Keys.K_S, world) != KeyState.KEY_UP)
            {
                moveInDir(ref world, ref map, ref pos, dir * -1);
            }
            if (InputManager.GetKey(Keys.K_D, world) != KeyState.KEY_UP)
            {
                moveInDir(ref world, ref map, ref pos, new Vector2d(-dir.y, dir.x) * -1);
            }
            if (InputManager.GetKey(Keys.K_A, world) != KeyState.KEY_UP)
            {
                moveInDir(ref world, ref map, ref pos, new Vector2d(-dir.y, dir.x));
            }
            if (InputManager.GetKey(Keys.K_RIGHT, world) != KeyState.KEY_UP)
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
            if (InputManager.GetKey(Keys.K_LEFT, world) != KeyState.KEY_UP)
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
            if (InputManager.GetKey(Keys.K_E, world) == KeyState.KEY_DOWN)
            {
                world.dayTime += 0.05f;
                if (world.dayTime > 1)
                    world.dayTime -= 1;

                if (spriteToInteract != null)
                    spriteToInteract.Activate(ref world);
            }
            if (InputManager.GetKey(Keys.K_ESC, world) == KeyState.KEY_DOWN)
            {
                GUI.GUI.pauseUIIndex = 1;
                world.state = states.Paused;
            }

            if (InputManager.GetKey(Keys.K_LCTRL, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_RCTRL, world) == KeyState.KEY_DOWN)
            {
                map.sprites.Add(new Fireball(world.plrPos.x, world.plrPos.y, 8, 6, $"100 {(int)(dir.x*1000)} {(int)(dir.y*1000)}"));
                map.lightPoitions.Add(map.sprites.Count - 1);
            }
        }

        public static void moveInDir(ref World world, ref Map map, ref Vector2d pos, Vector2d dir)
        {
            double movSpeed = world.dt * ((InputManager.GetKey(Keys.K_SHIFT, world) != KeyState.KEY_UP && world.staminaLVL > 0) ? 2 : 1);

            if (InputManager.GetKey(Keys.K_SHIFT, world) != KeyState.KEY_UP && world.staminaLVL > 0)
            {
                world.staminaLVL -= world.dt / 2;
                if (world.staminaLVL < 0)
                    world.staminaLVL = -0.2f;
            }

            float extraColDistMult = 1f;

            // CellX and CellY holds the cell, the player would move
            // into, in those directions. Using a vector doesn't
            // make sense, since they could be different. They are
            // split up, to allow sliding on walls, when not walking
            // perpendicular into them.
            Wall cellX = map.GetWall((int)(pos.x + dir.x * (movSpeed * extraColDistMult)), (int)(pos.y));
            Wall cellY = map.GetWall((int)(pos.x), (int)(pos.y + dir.y * (movSpeed * extraColDistMult)));

            // Check if cell is empty or a control cell, if so, move.
            if (!cellX.isWall) pos.x += dir.x * movSpeed;
            if (!cellY.isWall) pos.y += dir.y * movSpeed;

            cellX.Collide(ref world);
            cellY.Collide(ref world);
        }

        public static void WallCasting(ref ConsoleBuffer game, ref double[] ZBuffer, Vector2d dir, Vector2d plane, Vector2d pos, float visRange, Map map)
        {

            int width = game.GetWinWidth();
            int height = game.GetWinHeight();
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


            // Parallel.For(0, game.GetWinWidth(),
            //              x => {
            //                  casted[x] = DoOneWallcast(x, width, height, lights, dir, plane, pos, visRange, map);
            //                  });

            for(int x = 0; x < game.GetWinWidth(); x++) {
                casted[x] = DoOneWallcast(x, width, height, lights, dir, plane, pos, visRange, map);
            }

            for (int x = 0; x < game.GetWinWidth(); x++) {
                WallcastReturn cast = casted[x];
                // Draw the ray.
                if (cast.HitWall.doDraw) {
                    game.DrawVerLine(x, cast.LineHeight, cast.Tex, cast.TexX, cast.Darken, cast.MixedLight, null);
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

        public static WallcastReturn DoOneWallcast(int x, int width, int height, ILight[] lights, Vector2d dir, Vector2d plane, Vector2d pos, float visRange, Map map, double alreadyDist = 0, int recurseCount = 0) {
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

            // Calculate the map position of the ray hit.
            Vector2d hitPos = new Vector2d(
                pos.x + perpWallDist * rayDir.x,
                pos.y + perpWallDist * rayDir.y
            );

            if(map.world.dayTime > 0.5f) {
                darken *= 0.6f;
            } else {
                Vector2d realPosAbove = new Vector2d(hitPos.x + 0.1, hitPos.y);
                const float offset = 20;
                realPosAbove.y += map.world.dayTime * offset - offset/4;
                Vector2i cellPosAbove = (Vector2i)realPosAbove;
                if(map.GetRoof(cellPosAbove.x, cellPosAbove.y) != 0 || map.IsWall(cellPosAbove.x, cellPosAbove.y))
                    darken *= 0.6f;
            }

            if(hitWall.wallID == 5 && recurseCount < 5) {
                Vector2d newDir;
                if(side == 0)
                    newDir = new Vector2d(-dir.x, dir.y);
                else
                    newDir = new Vector2d(dir.x, -dir.y);
                return DoOneWallcast(x, width, height, lights, newDir, plane, hitPos, visRange, map, perpWallDist + alreadyDist, recurseCount+1);
            }
            // Do Lighting
            // ===========

            LightDist[] lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, hitPos);
            TexColor mixedLight = new TexColor(255, 255, 255);
            // if(lights.Count() <= 0) {
                mixedLight = LightDistHelpers.MixLightDist(lightDists);
            // }


            return new WallcastReturn(lineHeight, tex, texX, darken, perpWallDist + alreadyDist, mixedLight, hitWall);
        }

        public static void FloorCasting(ref Texture game, Vector2d dir, Vector2d plane, Vector2d pos, float visRange, Map map, World world)
        {
            game.Clear();

            Map curMap = world.getMapByID(world.currentMap);
            ILight[] lights = map.GetLights();

            // Grabs the floor and ceiling texture, before the loop, since we
            // don't want differently textured ceiling or floor.

            Texture floorTex   = textures[map.floorTexID];
            Texture ceilingTex = textures[map.useSkybox ? 1 : map.ceilTexID];

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
                Vector2d rayDirLeft = dir - plane;
                Vector2d rayDirRight = dir + plane;

                // Calculate the current rows offset from the middle of the
                // screen.
                int midOff = y - (winHeight / 2)+1;
                // Get the camera height, assuming it to be in the middle of the
                // screen.
                float camHeight = 0.5f * winHeight;
                float lineDist = camHeight / midOff;
                // Cap lineDist, since it'll be casted to an int later.
                lineDist = lineDist < 1000000000 ? lineDist : 1000000000;

                Vector2d floorOff = lineDist * (rayDirRight - rayDirLeft) / winWidth;

                Vector2d floor = pos + (new Vector2d(lineDist, lineDist) * rayDirLeft);

                for(int x = 0; x < winWidth; x++) {
                    if(lights.Count() > 0) {
                        lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, floor);
                        mixedLight = LightDistHelpers.MixLightDist(lightDists);
                    }

                    Vector2i cellPos = (Vector2i)floor.Floor();
                    int floorId = curMap.GetFloor(cellPos.x, cellPos.y);
                    floorTex = floorId == 0 ? null : textures[floorId];

                    int ceilId = curMap.GetRoof(cellPos.x, cellPos.y);
                    ceilingTex = ceilId == 0 ? null : textures[ceilId];

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
                        color  = texColor * darken * 0.7f;
                        color += TexColor.unitMult(texColor, mixedLight) * 0.3f;
                        if(world.dayTime > 0.5f) {
                            color += new TexColor(-50, -50, -50);
                        } else {
                            Vector2d realPosAbove = new Vector2d(floor.x + 0.1, floor.y);
                            const float offset = 20;
                            realPosAbove.y += world.dayTime * offset - offset/4;
                            Vector2i cellPosAbove = (Vector2i)realPosAbove;
                            if(curMap.GetRoof(cellPosAbove.x, cellPosAbove.y) != 0 || curMap.IsWall(cellPosAbove.x, cellPosAbove.y))
                                color *= 0.6f;
                        }
                        game.setPixel(x, y, color);
                    }

                    // Ceiling code
                    // ============
                    if(ceilingTex is null) {
                        var pix = GetSkyboxPixel(winHeight, dir, textures[99], x, winHeight - y - 1, world.dayTime);
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
                        color *= 0.7f * 0.6f;
                        color += TexColor.unitMult(texColor, mixedLight) * 0.3f;
                        game.setPixel(x, winHeight - y - 5, color * 0.20f);
                        game.setPixel(x, winHeight - y - 4, color * 0.50f);
                        game.setPixel(x, winHeight - y - 3, color * 0.70f);
                        game.setPixel(x, winHeight - y - 2, color * 0.90f);
                        game.setPixel(x, winHeight - y - 1, color * 1.00f);
                    }

                    floor += floorOff;
                }
            }
        }

        // Draws the skybox to the top half of the game screen. This isn't very
        // optimized, and shouldn't be used, as it draws to pixels, that will
        // later be drawn over.
        public static void DrawSkybox(ref ConsoleBuffer game, Vector2d dir, Texture skyboxTex, World world) {
            int winHeight = game.GetWinHeight();

            for(int y = 0; y < game.GetWinHeight() / 2; y++) {
                for(int x = 0; x < game.GetWinWidth(); x++) {
                    var pix = GetSkyboxPixel(winHeight, dir, skyboxTex, x, y, world.dayTime);
                    game.DrawPixel(pix, x, y);
                }
            }
        }

        public static TexColor GetSkyboxPixel(int winHeight, Vector2d dir, Texture skyboxTex, int x, int y, float dayTime) {
            // The difference between the height of on pixel on the screen and
            // on the texture, were the texture to fill the top helf of the
            // screen.
            double heightDiff = skyboxTex.height / (winHeight * 0.5);
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
            while(cal.x < 0)
                cal.x += skyboxTex.width;

            /* This no work :/, we try anohter try, no day-night for us *sadge*
            // getting the rotational position of the pixle
            float rot = -|-;

            // placing the point in 3d
            float nX = MathF.Cos(rot);
            float nY = cal.y;
            float nZ = MathF.Sin(rot);

            // and then back to 2d, lol
            float x2D = nZ*80; // also map this so that it seems better
            float y2D = nY;

            // mapping the time to pi
            float timeRot = (dayTime / 1) * MathF.PI * 2;

            // getting rot of the 2d point
            float rot2D = MathF.Atan2(skyboxTex.height-y2D, x2D); // *2 for pix2

            if (rot2D < timeRot && rot2D+MathF.PI > timeRot)
                return skyboxTex.getPixel(cal.x, cal.y)*0.2f;
            */

            // Return the pixel at the position.
            return skyboxTex.getPixel(cal.x, cal.y);


        }

        public static void SpriteCasting(ref ConsoleBuffer game, List<Sprite> sprites, Vector2d pos, Vector2d plane, Vector2d dir, double[] ZBuffer, int visRange, Map map, ref World world) {
            ILight[] lights = map.GetLights();

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

                    if (transformed.y < ZBuffer[x])
                    {
                        curSpr.UpdateOnDraw(ref world, transformed.y);
                        TexColor mixedLight = new TexColor(255, 255, 255);
                        Vector2d newPlane = ((plane*-1) + ((plane*2))/(endX - startX)*(x-startX));

                        LightDist[] lightDists = LightDistHelpers.RoofLightArrayToDistArray(lights, curSpr.pos + newPlane);
                        if(lights.Count() < 0)
                            mixedLight = LightDistHelpers.MixLightDist(lightDists);

                        if(world.dayTime > 0.5f) {
                            darken *= 0.6f;
                        } else {
                            Vector2d realPosAbove = new Vector2d(curSpr.pos.x, curSpr.pos.y);

                            const float offset = 20;
                            realPosAbove.x += 0.1;
                            realPosAbove.y += world.dayTime * offset - offset/4;
                            Vector2i cellPosAbove = (Vector2i)realPosAbove;
                            if(map.GetRoof(cellPosAbove.x, cellPosAbove.y) != 0 || map.IsWall(cellPosAbove.x, cellPosAbove.y))
                                darken *= 0.6f;
                        }

                        game.DrawVerLine(x, spriteScreenSize, sprTex, texX, darken, mixedLight, new TexColor(0, 0, 0));
                    }
                }
            }
        }
    }
}
