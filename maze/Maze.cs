using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
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

namespace textured_raycast.maze
{
    class Maze {

        static Button[] invButtons = new Button[]
        {
            new Back(1, 1, 11, 11, new int[] {0, 1, 4, 0}), new Skills(13, 1, 23, 11, new int[] {0, 0, 1, -1}),
            new PlaceHolder(13, 18, 11, 11, new int[] {-1, 1, 3, 0}), new PlaceHolder(44, 18, 11, 11, new int[] {0, 97, 4, -1}),
            new PlaceHolder(1, 30, 11, 11, new int[] {-2, 1, 4, 0}), new PlaceHolder(13, 30, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(25, 30, 11, 11, new int[] {-4, 1, 3, -1}), new PlaceHolder(44, 30, 11, 11, new int[] {-4, 98, 3, -1}),
            new PlaceHolder(1, 42, 11, 11, new int[] {-4, 1, 0, 0}), new PlaceHolder(25, 42, 11, 11, new int[] {-3, 1, 0, -1}), new PlaceHolder(44, 42, 11, 11, new int[] {-3, 100, 0, -1})
        };

        static Button[] skillButtons = new Button[]
        {
            new SkillPlaceHolder(255, 5, 21, 21, new int[] {0, 0, 2, 0}),
            new SkillPlaceHolder(153, 56, 21, 21, new int[] {0, 1, 9, 0}), new SkillPlaceHolder(255, 56, 21, 21, new int[] {-2, 1, 3, -1}), new SkillPlaceHolder(357, 56, 21, 21, new int[] {0, 0, 11, -1}),
            new SkillPlaceHolder(0, 107, 21, 21, new int[] {0, 0, 3, 0}), new SkillPlaceHolder(255, 107, 21, 21, new int[] {-3, 0, 7, 0}), new SkillPlaceHolder(510, 107, 21, 21, new int[] {0, 0, 11, 0}),

            new SkillPlaceHolder(0, 158, 21, 21, new int[] {-3, 1, 11, 0}), new SkillPlaceHolder(51, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(102, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(153, 158, 21, 21, new int[] {-9, 1, 11, -1}), new SkillPlaceHolder(204, 158, 21, 21, new int[] {0, 1, 0, -1}),
            new SkillPlaceHolder(255, 158, 21, 21, new int[] {-7, 1, 7, -1}),
            new SkillPlaceHolder(306, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(357, 158, 21, 21, new int[] {-11, 1, 9, -1}), new SkillPlaceHolder(408, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(459, 158, 21, 21, new int[] {0, 1, 0, -1}), new SkillPlaceHolder(510, 158, 21, 21, new int[] {-11, 0, 3, -1}),

            new SkillPlaceHolder(0, 209, 21, 21, new int[] {-11, 0, 0, 0}), new SkillPlaceHolder(255, 209, 21, 21, new int[] {-7, 0, 3, 0}), new SkillPlaceHolder(510, 209, 21, 21, new int[] {-3, 0, 0, 0}),
            new SkillPlaceHolder(153, 260, 21, 21, new int[] {-11, 1, 0, 0}), new SkillPlaceHolder(255, 260, 21, 21, new int[] {-3, 1, 2, -1}), new SkillPlaceHolder(357, 260, 21, 21, new int[] {-9, 0, 0, -1}),
            new SkillPlaceHolder(255, 310, 21, 21, new int[] {-2, 0, 0, 0}),
        };

        static Vector2i size = new Vector2i(120, 80);

        static ConsoleEngine engine;
	static ConsoleBuffer game;
	static ConsoleBuffer fight;
	static ConsoleBuffer UIHolder;

        public static bool StartMaze(World world) {
            engine = new ConsoleEngine(size.x, size.y, "maze");
            game = new ConsoleBuffer(size.x, size.y);
            fight = new ConsoleBuffer(size.x, size.y);
            UIHolder = new ConsoleBuffer(size.x, size.y);

            Console.Clear();
            DrawScreen(engine);

            return Start(world);
        }

        private static bool Start(World world) {

            Map map = world.getMapByID(world.currentMap);

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

            double[] ZBuffer = new double[engine.Width];

            int curInvButton = 0;
            int curSkillButton = 12;


            Texture FloorAndRoof = new Texture(size.x, size.y);
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
                    }
                    else
                    {
                        UIHolder.Clear();
                        world.fight.renderFightStartScreenToBuffer(ref UIHolder, world.fight.tillFightBegins / 2 - 0.1f);

                        engine.DrawConBuffer(game.mixBuffer(UIHolder));
                        engine.SwapBuffers();
                    }
                }

                while (world.state == states.Inventory)
                {
                    UIHolder.Clear();

                    UIHolder.DrawTexture(ResourceManager.getTexture(world.textures[103]), 0, 0);

                    int nowInv = -1;
                    if (curInvButton > 99) nowInv = curInvButton - 100;


                    if (nowInv == -1)
                    {
                        if (InputManager.GetKey(Keys.K_UP, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_W, world) == KeyState.KEY_DOWN)
                        {
                            curInvButton += invButtons[curInvButton].listOfMovements[0];
                        }
                        if (InputManager.GetKey(Keys.K_DOWN, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_S, world) == KeyState.KEY_DOWN)
                        {
                            curInvButton += invButtons[curInvButton].listOfMovements[2];
                        }
                        if (InputManager.GetKey(Keys.K_RIGHT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_D, world) == KeyState.KEY_DOWN)
                        {
                            curInvButton += invButtons[curInvButton].listOfMovements[1];
                        }
                        if (InputManager.GetKey(Keys.K_LEFT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_A, world) == KeyState.KEY_DOWN)
                        {
                            curInvButton += invButtons[curInvButton].listOfMovements[3];
                        }
                        if (InputManager.GetKey(Keys.K_E, world) == KeyState.KEY_DOWN)
                        {
                            invButtons[curInvButton].onActivate(world);
                        }
                        if (InputManager.GetKey(Keys.K_ESC, world) == KeyState.KEY_DOWN)
                        {
                            curInvButton = 0;
                            world.state = states.Paused;
                        }
                    }
                    else
                    {
                        int y = (int)Math.Floor(nowInv / 5f);
                        int x = nowInv - y * 5;
                        int pageOffset = (int)Math.Floor(y / 5f);
                        y = y - pageOffset * 5;

                        if (InputManager.GetKey(Keys.K_UP, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_W, world) == KeyState.KEY_DOWN)
                        {
                            if (InputManager.GetKey(Keys.K_SHIFT, world) != KeyState.KEY_UP)
                                pageOffset = Math.Max(pageOffset-1, 0);
                            else
                                y -= 1;
                            if (y == -1 && pageOffset == 0) y = 0;
                        }
                        if (InputManager.GetKey(Keys.K_DOWN, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_S, world) == KeyState.KEY_DOWN)
                        {
                            if (InputManager.GetKey(Keys.K_SHIFT, world) != KeyState.KEY_UP)
                                pageOffset += 1;
                            else
                                y += 1;
                        }
                        if (InputManager.GetKey(Keys.K_RIGHT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_D, world) == KeyState.KEY_DOWN)
                        {
                            x = Math.Min(x+1, 4);
                        }
                        if (InputManager.GetKey(Keys.K_LEFT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_A, world) == KeyState.KEY_DOWN)
                        {
                            if (x == 0)
                            {
                                if (y == 0)
                                    curInvButton = 3;
                                else if (y == 1)
                                    curInvButton = 7;
                                else if (y == 2)
                                    curInvButton = 10;
                                else
                                    curInvButton = 10;
                            }

                            x -= 1;
                        }

                        if (curInvButton > 99)
                        {
                            curInvButton = x + y * 5 + pageOffset * 25 + 100;
                        }
                    }

                    nowInv = -1;
                    if (curInvButton > 99) nowInv = curInvButton - 100;


                    if (nowInv == -1)
                    {
                        UIHolder = invButtons[curInvButton].DrawOnBuffer(UIHolder);
                    }
                    else
                    {
                        int y = (int)Math.Floor(nowInv / 5f);
                        int x = nowInv - y * 5;
                        int pageOffset = (int)Math.Floor(y / 5f);
                        y = y - pageOffset * 5;

                        GUI.GUI.text(ref UIHolder, ref world, "abcdefghijklmnopqrstuvwxyz", 58, 3, 59);

                        UIHolder = new PlaceHolder(58 + x * 12, 18 + y * 12, 11, 11, new int[] { }).DrawOnBuffer(UIHolder);
                    }

                        engine.DrawConBuffer(UIHolder);
                    engine.SwapBuffers();
                }

                while (world.state == states.Skills)
                {
                    UIHolder.Clear();

                    for (int x = 0; x < 120; x++)
                        for (int y = 0; y < 80; y++)
                            UIHolder.DrawPixel(new TexColor(198, 132, 68), x, y);

                    if (InputManager.GetKey(Keys.K_UP, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_W, world) == KeyState.KEY_DOWN)
                    {
                        curSkillButton += skillButtons[curSkillButton].listOfMovements[0];
                    }
                    if (InputManager.GetKey(Keys.K_DOWN, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_S, world) == KeyState.KEY_DOWN)
                    {
                        curSkillButton += skillButtons[curSkillButton].listOfMovements[2];
                    }
                    if (InputManager.GetKey(Keys.K_RIGHT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_D, world) == KeyState.KEY_DOWN)
                    {
                        curSkillButton += skillButtons[curSkillButton].listOfMovements[1];
                    }
                    if (InputManager.GetKey(Keys.K_LEFT, world) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_A, world) == KeyState.KEY_DOWN)
                    {
                        curSkillButton += skillButtons[curSkillButton].listOfMovements[3];
                    }
                    if (InputManager.GetKey(Keys.K_E, world) == KeyState.KEY_DOWN)
                    {
                        skillButtons[curSkillButton].onActivate(world);
                    }
                    if (InputManager.GetKey(Keys.K_ESC, world) == KeyState.KEY_DOWN)
                    {
                        curSkillButton = 12;
                        world.state = states.Inventory;
                    }

                    UIHolder.DrawTexture(ResourceManager.getTexture(world.textures[104]), 60 - skillButtons[curSkillButton].x - skillButtons[curSkillButton].w / 2, 40 - skillButtons[curSkillButton].y - skillButtons[curSkillButton].w / 2);


                    for (int x = 1; x < 26; x++)
                    {
                        for (int y = 62; y < 79; y++)
                        {
                            TexColor tc = new TexColor(198, 132, 68);
                            if(x == 1 || x == 25 || y == 62 || y == 78)
                                tc = new TexColor(0, 0, 0);

                            UIHolder.DrawPixel(tc, x, y);
                        }
                    }

                    int i = 0;
                    foreach (Button sb in skillButtons)
                    {
                        if (i == curSkillButton)
                            UIHolder.DrawPixel(new TexColor(0, 0, 255), (sb.x / 51) * 2 + 3, (sb.y / 51) * 2 + 64);
                        else
                            UIHolder.DrawPixel(new TexColor(0, 0, 0), (sb.x / 51) * 2 + 3, (sb.y / 51) * 2 + 64);
                        i++;
                    }


                    engine.DrawConBuffer(UIHolder);
                    engine.SwapBuffers();
                }

                while (world.state == states.Paused)
                {
                    UIHolder.Clear();

                    GUI.GUI.pauseGUI(ref UIHolder, ref world);

                    engine.DrawConBuffer(game.mixBuffer(UIHolder));
                    engine.SwapBuffers();
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
                    FloorCasting.FloorCast(ref FloorAndRoof, dir, plane, pos, visRange, map, world, world.textures);
                    game.DrawTexture(FloorAndRoof, 0, 0);

                    // Do the wall casting
                    WallCasting.WallCast(ref game, ref ZBuffer, dir, plane, pos, visRange, map, world.textures);

                    // draw sprites
                    SpriteCasting.SpriteCast(ref game, map.sprites, pos, plane, dir, ZBuffer, visRange, map, ref world);

                    engine.DrawConBuffer(game.mixBuffer(UIHolder));
                    engine.SwapBuffers();
                    HandleInputGame(ref world, map, pos, ref dir, ref plane, ref spriteToInteract);
                }
            }

            return false;
        }

        private static TexColor TexColor(int v1, int v2, int v3)
        {
            throw new NotImplementedException();
        }

        // Multi-threaded screen-drawing
        // =============================
        // Draw the screen asynchronously
        public static void DrawScreen(ConsoleEngine engine) {
            Task.Run(() => {
                while(true)
                    engine.DrawScreen();
            });
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


    }
}
