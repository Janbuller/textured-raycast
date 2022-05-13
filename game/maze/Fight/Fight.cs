using System;
using System.Collections.Generic;
using textured_raycast.maze.graphics;
using textured_raycast.maze.math;
using textured_raycast.maze.skills;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;
using textured_raycast.maze.input;

namespace textured_raycast.maze.Fight
{
    internal class Fight
    {
        string[] textures;
        int mapID = -1;

        public void link(ref ConsoleBuffer game, ref ConsoleBuffer fight, ref ConsoleBuffer UIHolder)
        {
            this.game = game;
            this.fight = fight;
            this.UIHolder = UIHolder;
        }

        // make a lot of variables
        Random r = new Random();

        // to save the enemy
        Enemy sTF;

        // all the buffer layers to draw on (for layering, mostly ui)
        ConsoleBuffer game;
        ConsoleBuffer fight;
        ConsoleBuffer UIHolder;

        // for haveing a fun pokemon clone like animation
        public float tillFightBegins = 2;

        // hp and max hp of monster
        public float hp;
        public float maxHp;

        // fight variables, if the player activates skills that effect the fight for a certain amount of time
        public int dodgeStance = 0;
        public int bezerk = 0;
        public bool chargeSpell = false;
        public int poison = 0;

        // spites in the map (notmally only the one enemy, but its needed to work with the draw since it takes a list)
        // draw as in sprite cast
        List<Sprite> sprites = new List<Sprite>();

        // rot pos and plane(used to make the raycast look not doo doo) of the player
        Vector2d startRot;
        Vector2d startPos;
        Vector2d startPlane;

        public void initiateNewFight(Enemy spriteToFight)
        {
            // save the enemy
            sTF = spriteToFight;

            // save its image and hp as well
            textures = sTF.texture;
            maxHp = sTF.hp;
            hp = maxHp;

            // reset combat/fight variables
	        dodgeStance = 0;
	        bezerk = 0;
	        chargeSpell = false;
            poison = 0;

            // make the list only contain the enemy
            sprites = new List<Sprite>();
            sprites.Add(new DefaultSprite(2.35, 2, textures));
        }

        public void damMon(int dam)
        {
            // dam monster, and heal player if he/she has life steal skill
            hp -= dam;
            World.player.actualHp = MathF.Min(World.player.actualHp + MathF.Ceiling(dam * ((float)World.player.addPLifeSteal / 100f)), World.player.Hp);
        }

        public void enemyDoAction()
        {
            // gets amoungof damage the monster will do, if it hits
            float dam = (sTF.appDamage + (float)r.Next((int)sTF.damageVar * -1, (int)sTF.damageVar));

            if (dodgeStance != 0)
            {
                dodgeStance -= 1; // one less turn of dodge stance
                if (r.Next(0, 1) == 1) // check if it hits
                {
                    if (World.player.don) // if you have double damage or no damge
                    {
                        if (r.Next(0, 1) == 1) // check if you take double or no damage
                            World.player.actualHp -= dam;
                    }
                    else
                        World.player.actualHp -= dam; // deal damage
                }
            }
            else
            {
                // same as above
                if (World.player.don)
                {
                    if (r.Next(0, 1) == 1)
                        World.player.actualHp -= dam;
                }
                else
                    World.player.actualHp -= dam;
            }

            if (World.player.actualHp <= 0) // if the players hp is equals or less than 0, make him/her die
                plrDead();

            // make the monster take damage form poisiom, then make the poison go down
            World.fight.hp -= poison;
            poison = Math.Max(poison, 0);
        }

        public void enemyDead()
        {
            // if the enemy is dead, return to normal game state
            World.state = States.Game;

            // add xp and money
            World.player.xp += sTF.xpGiven;
            World.player.money += sTF.moneyRecived + r.Next(-((int)sTF.moneyVar), (int)sTF.moneyVar);

            // level up if it can
            while (World.player.xp > (MathF.Pow(1.1f, World.player.lvl) * 100 - 10))
            {
                World.player.xp -= (MathF.Pow(1.1f, World.player.lvl) * 100 - 10);
                World.player.lvl++;
                World.player.skillPoints++;
            }

            // make the spite invisible, and non interactable
            sTF.canInteract = false;
            sTF.doRender = false;
        }

        public void plrDead()
        {
            // back to normal game state
            World.state = States.Game;

            // reset the player position
            World.player.reset();
            // and reset the map (monsters mostly, chests dont reset)
            World.getCurMap().fullReset();
        }

        public void renderFightToBuffer(ref ConsoleBuffer buffer)
        {
            // save world player transform
	        startRot = World.plrRot;
	        startPos = World.plrPos;
	        startPlane = World.plrPlane;

            // set it to a new fixed position
            World.plrRot = new Vector2d(-1, 0);
            World.plrPos = new Vector2d(3.65, 2);
            World.plrPlane = new Vector2d(World.plrRot.Y, -World.plrRot.X) * 0.66;
            
            // get map, by id, 0, fight map...
            Map map = World.getMapByID(mapID);

            // get floor, and place it on the buffer
            Texture FloorAndRoof = new Texture(buffer.Width, buffer.Height);
            FloorCasting.FloorCast(ref FloorAndRoof, 1, map);
            buffer.DrawTexture(FloorAndRoof, 0, 0);

            // make a zbuffer for drawing sprites behind walls
            double[] ZBuffer = new double[buffer.Width];

            // get zbuffer and draw wallcast
            WallCasting.WallCast(ref buffer, ref ZBuffer, 1, map);

            // update sprites to draw
            foreach (Sprite sprite in sprites)
            {
                sprite.updateAnimation();
            }

            // draw spites
            SpriteCasting.SpriteCast(ref buffer, sprites, ZBuffer, 1, map);

            // draw boxes
            buffer.DrawBox(1, 1, 60, 6 , new TexColor(0, 0, 0));
            buffer.DrawBox(59, 73, 60, 6, new TexColor(0, 0, 0));

            // draw the same boxes, but with a small part cut off, to represeent hp of monster and player
            buffer.DrawBox(2, 2, (int)(58 * (hp / maxHp)), 4, new TexColor(0, 255, 0));
            buffer.DrawBox(60, 74, (int)(58 * (World.player.actualHp / World.player.Hp)), 4, new TexColor(0, 255, 0));

            // draw equipped skills
            for (int i = 0; i < 3; i++)
            {
                if (World.player.equippedSkills[i] != -1)
                {
                    buffer.DrawBox(1 + i * 12, 68, 11, 11, new TexColor(0, 0, 0));
                    buffer.DrawTexture(Skill.Skills[World.player.equippedSkills[i]].getTexture(), 2 + i * 13, 69, new TexColor(0, 0, 0));
                }
            }

            // set tranform back
            World.plrRot = startRot;
            World.plrPos = startPos;
            World.plrPlane = startPlane;
        }

        public bool doFight()
        {
            // make a loop for the fight
            while (World.state == States.Fighting)
            {
                // minus animation
                World.fight.tillFightBegins -= (float)World.dt;

                // if animation is not active
                if (World.fight.tillFightBegins < 0)
                {
                    fight.Clear();

                    // try to use skills if you press the corresponding button
                    if (InputManager.GetKey(Keys.K_1) == KeyState.KEY_DOWN)
                        World.player.useSkill(0);
                    if (InputManager.GetKey(Keys.K_2) == KeyState.KEY_DOWN)
                        World.player.useSkill(1);
                    if (InputManager.GetKey(Keys.K_3) == KeyState.KEY_DOWN)
                        World.player.useSkill(2);

                    // there are some self damaginf skills so
                    if (World.player.actualHp <= 0)
                        plrDead(); // check for suicide
                    else
                    {
                        // draw screen
                        World.fight.renderFightToBuffer(ref fight);

                        World.ce.DrawConBuffer(fight);
                        World.ce.SwapBuffers();
                    }
                }
                else
                {
                    // draw the animation
                    UIHolder.Clear();
                    World.fight.renderFightStartScreenToBuffer(ref UIHolder, World.fight.tillFightBegins / 2 - 0.1f);

                    World.ce.DrawConBuffer(game.mixBuffer(UIHolder));
                    World.ce.SwapBuffers();
                }
            }

            if (World.fight.hp <= 0)
                return true;

            return false;
        }

        public void renderFightStartScreenToBuffer(ref ConsoleBuffer buffer, float progress)
        {
            // basically draw a rotating line from pokemin, with trigonometry n stuff
            for (int x = 0; x < buffer.Width; x++)
            {
                for (int y = 0; y < buffer.Height; y++)
                {
                    if ((MathF.Atan2(buffer.Height / 2 - y, buffer.Width / 2 - x)/MathF.PI+1)/2 > progress)
                    {
                        buffer.DrawPixel(new textured_raycast.maze.texture.TexColor(0, 0, 0), x, y);
                    }
                }
            }
        }
    }
}
