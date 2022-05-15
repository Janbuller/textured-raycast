using textured_raycast.maze.math;
using textured_raycast.maze.graphics;
using textured_raycast.maze.texture;
using textured_raycast.maze.skills;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using System.Threading.Tasks;
using textured_raycast.maze.ButtonList;
using textured_raycast.maze.ButtonList.Buttons.INV;
using textured_raycast.maze.ButtonList.Buttons.Skills;
using System;

namespace textured_raycast.maze.DrawingLoops
{
    class GameLoop
    {
        static Map map = World.getCurMap();

        // The visibility distance. Controls the distance-based darkening.
        static int visRange = 25;

        static double[] ZBuffer = new double[World.ce.Width];

        static Texture FloorAndRoof = new Texture(World.WindowSize.X, World.WindowSize.Y);

        public static void GameLoopIter(ref ConsoleBuffer game, ref ConsoleBuffer UIHolder)
        {
            // Make time pass
            World.dayTime += (float)World.dt / 60; // 60 = 1 whole day = 60 sec
            World.timePassed += World.dt;

            if (World.dayTime > 1) World.dayTime--;

            if (InputManager.GetKey(Keys.K_SHIFT) == KeyState.KEY_UP)
            {
                World.staminaLVL = MathF.Min(World.staminaLVL + (float)World.dt / 4, 1 + MathF.Log10(MathF.Pow(World.player.lvl, 2)));
            }

            // make sure it knows what map its on
            map = World.getCurMap();
            visRange = map.useSkybox ? 1 : 25;

            // find closest sprite that is interactable and display interact message
            Sprite spriteToInteract = null;
            double distanceToInteract = 9999;

            for (int i = 0; i < map.sprites.Count; i++)
            {
                Sprite sprite = map.sprites[i];
                sprite.Update();

                sprite.updateAnimation();
        
                double distance = World.plrPos.DistTo(sprite.getPos());
                // Console.WriteLine(distance + " : " + sprite.canInteract);
                if (distance < sprite.interactDistance && distance < distanceToInteract && sprite.canInteract)
                {
                    if (sprite.autoInteract)
                    {
                        sprite.Activate();
                    }
                    else
                    {
                        spriteToInteract = sprite;
                        distanceToInteract = distance;
                    }
            }
            }

            // if it found one, display interact message, otherwise dont...
            if (spriteToInteract != null)
                World.interactMessage = spriteToInteract.ActivateMessage();
            else
                World.interactMessage = "";

            // if there is no current message, show interaction message, otherwise show current message
            string toSend = World.currentMessage == "" ? World.interactMessage : World.currentMessage;

            //Clear the UI buffer
            UIHolder.Clear();

            // Add text-box to UI buffer if there is a string that should go in it.
            if (toSend != "")
            {
                GUI.GUI.texBox(ref UIHolder, toSend);
            }

            // Draw the stamina bar in the UI Buffer
            for (int i = 0; i < 78; i++)
            {
                if (World.staminaLVL > ((float)i / 78) * (1 + MathF.Log10(MathF.Pow(World.player.lvl, 2))))
                {
                    UIHolder.DrawPixel(new TexColor(0, 155, 0), 117, 1 + i);
                    UIHolder.DrawPixel(new TexColor(0, 155, 0), 118, 1 + i);
                }
            }

            // Do the floor/ceiling casting.
            FloorCasting.FloorCast(ref FloorAndRoof, visRange);
            game.DrawTexture(FloorAndRoof, 0, 0);

            // Do the wall casting
            WallCasting.WallCast(ref game, ref ZBuffer, visRange);

            // draw sprites
            SpriteCasting.SpriteCast(ref game, map.sprites, ZBuffer, visRange, map);

            World.ce.DrawConBuffer(game.mixBuffer(UIHolder));
            World.ce.SwapBuffers();
            HandleInput(map, World.plrPos, ref World.plrRot, ref spriteToInteract);
        }

        public static void HandleInput(Map map, Vector2d pos, ref Vector2d dir, ref Sprite spriteToInteract)
        {
            double rotSpeed = World.dt * 1.5;

            // Multiplied with movement speed, during collision check,
            // forcing the player to stay slightly further away from
            // walls.

            // keypresses, for moving
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_UP]) != KeyState.KEY_UP)
            {
                moveInDir(ref map, ref pos, dir, true);
            }
            if (InputManager.GetKeyGroup(InputManager.KeyGroup[KeyGroups.KG_DOWN]) != KeyState.KEY_UP)
            {
                moveInDir(ref map, ref pos, dir * -1, true);
            }
            if (InputManager.GetKey(Keys.K_D) != KeyState.KEY_UP)
            {
                moveInDir(ref map, ref pos, new Vector2d(-dir.Y, dir.X) * -1);
            }
            if (InputManager.GetKey(Keys.K_A) != KeyState.KEY_UP)
            {
                moveInDir(ref map, ref pos, new Vector2d(-dir.Y, dir.X));
            }
            if (InputManager.GetKey(Keys.K_RIGHT) != KeyState.KEY_UP)
            {
                // Use too much math, to calculate the direction unit vector.
                double oldDirX = dir.X;
                dir.X = dir.X * Math.Cos(-rotSpeed) - dir.Y * Math.Sin(-rotSpeed);
                dir.Y = oldDirX * Math.Sin(-rotSpeed) + dir.Y * Math.Cos(-rotSpeed);
                // Use too much math, to calculate the camera viewport plane.
                double oldPlaneX = World.plrPlane.X;
                World.plrPlane.X = World.plrPlane.X * Math.Cos(-rotSpeed) - World.plrPlane.Y * Math.Sin(-rotSpeed);
                World.plrPlane.Y = oldPlaneX * Math.Sin(-rotSpeed) + World.plrPlane.Y * Math.Cos(-rotSpeed);
            }
            if (InputManager.GetKey(Keys.K_LEFT) != KeyState.KEY_UP)
            {
                // Use too much math, to calculate the direction unit vector.
                double oldDirX = dir.X;
                dir.X = dir.X * Math.Cos(rotSpeed) - dir.Y * Math.Sin(rotSpeed);
                dir.Y = oldDirX * Math.Sin(rotSpeed) + dir.Y * Math.Cos(rotSpeed);
                // Use too much math, to calculate the camera viewport plane.
                double oldPlaneX = World.plrPlane.X;
                World.plrPlane.X = World.plrPlane.X * Math.Cos(rotSpeed) - World.plrPlane.Y * Math.Sin(rotSpeed);
                World.plrPlane.Y = oldPlaneX * Math.Sin(rotSpeed) + World.plrPlane.Y * Math.Cos(rotSpeed);
            }
            if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
            {
                // if you press e, and there is a sprite that you can interact with, interact with it
                if (spriteToInteract != null)
                    spriteToInteract.Activate();
            }
            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                // open pause state if you press esc
                GUI.GUI.pauseUIIndex = 1;
                World.state = States.Paused;
            }

            // check if the player has fireball, and if he dose, check if he is pressing the fireball key
            var FireballKey = World.player.FireballKey;
            if (!(FireballKey is null))
            {
                if (InputManager.GetKey(FireballKey.Value) == KeyState.KEY_DOWN)
                {
                    map.sprites.Add(new Fireball(World.plrPos.X, World.plrPos.Y, new string[] { "img/skills/fireball.ppm" }, 6, $"100 {(int)(World.plrRot.X * 1000)} {(int)(World.plrRot.Y * 1000)}"));
                }
            }
        }

        public static void moveInDir(ref Map map, ref Vector2d pos, Vector2d dir, bool doViewBob = false)
        {
            // get movement speed, double if you press shift and have stamina, multiply movementspeed by delta time, to make you run same speed no matter the fps
            double movSpeed = ((InputManager.GetKey(Keys.K_SHIFT) != KeyState.KEY_UP && World.staminaLVL > 0) ? 2 : 1);
            double curMovSpeed = World.dt * movSpeed;

            // do view bob, variable says it all
            if (doViewBob)
            {
                World.plrBobTime += (float)World.dt * 1000 * (float)movSpeed;
                World.plrBob = (float)(Math.Sin(World.plrBobTime * 0.01) + 1);
            }

            // if you dont press shift and have stamina, remove stamina
            if (InputManager.GetKey(Keys.K_SHIFT) != KeyState.KEY_UP && World.staminaLVL > 0)
            {
                World.staminaLVL -= (float)World.dt / 2;
                if (World.staminaLVL < 0)
                    World.staminaLVL = -0.2f;
            }

            float extraColDistMult = 1f;

            // CellX and CellY holds the cell, the player would move
            // into, in those directions. Using a vector doesn't
            // make sense, since they could be different. They are
            // split up, to allow sliding on walls, when not walking
            // perpendicular into them.
            Wall cellX = map.GetWall((int)(pos.X + dir.X * (curMovSpeed * extraColDistMult)), (int)(pos.Y));
            Wall cellY = map.GetWall((int)(pos.X), (int)(pos.Y + dir.Y * (curMovSpeed * extraColDistMult)));

            // Check if cell is empty or a control cell, if so, move.
            if (!cellX.isWall) pos.X += dir.X * curMovSpeed;
            if (!cellY.isWall) pos.Y += dir.Y * curMovSpeed;

            cellX.Collide();
            cellY.Collide();
        }
    }
}
