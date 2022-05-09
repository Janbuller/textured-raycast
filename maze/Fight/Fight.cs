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

        Random r = new Random();
        Enemy sTF;
        ConsoleBuffer game;
        ConsoleBuffer fight;
        ConsoleBuffer UIHolder;

        public float tillFightBegins = 2;

        public float hp;
        public float maxHp;

        public int dodgeStance = 0;
        public int bezerk = 0;
        public bool chargeSpell = false;
        public int poision = 0;

        List<Sprite> sprites = new List<Sprite>();

        Vector2d startRot;
        Vector2d startPos;
        Vector2d startPlane;

        public void initiateNewFight(Enemy spriteToFight)
        {
            sTF = spriteToFight;

            textures = sTF.texture;
            maxHp = sTF.hp;
            hp = maxHp;

            sprites = new List<Sprite>();
            sprites.Add(new DefaultSprite(2.35, 2, textures));
        }

        public void damMon(int dam)
        {
            hp -= dam;
            World.player.actualHp += dam * World.player.addPLifeSteal;
        }

        public void enemyDoAction()
        {
            float dam = (sTF.appDamage + (float)r.Next((int)sTF.damageVar * -1, (int)sTF.damageVar));

            if (dodgeStance != 0)
            {
                dodgeStance -= 1;
                if (r.Next(0, 1) == 1)
                {
                    if (World.player.don)
                    {
                        if (r.Next(0, 1) == 1)
                            World.player.actualHp -= dam;
                    }
                    else
                        World.player.actualHp -= dam;
                }
            }
            else
                World.player.actualHp -= dam;

            if (World.player.actualHp <= 0)
                plrDead();

            World.fight.hp -= poision;
            poision = Math.Max(poision, 0);
        }

        public void enemyDead()
        {
            World.state = States.Game;

            World.player.xp += sTF.xpGiven;
            World.player.money += sTF.moneyRecived + r.Next(-((int)sTF.moneyVar), (int)sTF.moneyVar);

            if (World.player.xp > (MathF.Pow(1.1f, World.player.lvl) * 100 - 10))
            {
                World.player.xp -= (MathF.Pow(1.1f, World.player.lvl) * 100 - 10);
                World.player.lvl++;
                World.player.skillPoints++;
            }

            sTF.canInteract = false;
            sTF.doRender = false;
        }

        public void plrDead()
        {
            World.state = States.Game;
            World.player.reset();
            World.getCurMap().fullReset();
        }

        public void renderFightToBuffer(ref ConsoleBuffer buffer)
        {
	        startRot = World.plrRot;
	        startPos = World.plrPos;
	        startPlane = World.plrPlane;

            World.plrRot = new Vector2d(-1, 0);
            World.plrPos = new Vector2d(3.65, 2);
            World.plrPlane = new Vector2d(World.plrRot.Y, -World.plrRot.X) * 0.66;
            Map map = World.getMapByID(mapID);

            Texture FloorAndRoof = new Texture(buffer.Width, buffer.Height);
            FloorCasting.FloorCast(ref FloorAndRoof, 1, map);
            buffer.DrawTexture(FloorAndRoof, 0, 0);

            double[] ZBuffer = new double[buffer.Width];

            WallCasting.WallCast(ref buffer, ref ZBuffer, 1, map);

            foreach (Sprite sprite in sprites)
            {
                sprite.updateAnimation();
            }

            SpriteCasting.SpriteCast(ref buffer, sprites, ZBuffer, 1, map);

            buffer.DrawBox(1, 1, 60, 6 , new TexColor(0, 0, 0));
            buffer.DrawBox(59, 73, 60, 6, new TexColor(0, 0, 0));

            buffer.DrawBox(2, 2, (int)(58 * (hp / maxHp)), 4, new TexColor(0, 255, 0));
            buffer.DrawBox(60, 74, (int)(58 * (World.player.actualHp / World.player.Hp)), 4, new TexColor(0, 255, 0));

            for (int i = 0; i < 3; i++)
            {
                if (World.player.equippedSkills[i] != -1)
                {
                    buffer.DrawBox(1 + i * 12, 68, 11, 11, new TexColor(0, 0, 0));
                    buffer.DrawTexture(Skill.Skills[World.player.equippedSkills[i]].getTexture(), 2 + i * 13, 68, new TexColor(0, 0, 0));
                }
            }

            World.plrRot = startRot;
            World.plrPos = startPos;
            World.plrPlane = startPlane;
        }

        public bool doFight()
        {
            while (World.state == States.Fighting)
            {
                World.fight.tillFightBegins -= (float)World.dt;

                if (World.fight.tillFightBegins < 0)
                {
                    fight.Clear();

                    if (InputManager.GetKey(Keys.K_1) == KeyState.KEY_DOWN)
                        World.player.useSkill(0);
                    if (InputManager.GetKey(Keys.K_2) == KeyState.KEY_DOWN)
                        World.player.useSkill(1);
                    if (InputManager.GetKey(Keys.K_3) == KeyState.KEY_DOWN)
                        World.player.useSkill(2);

                    if (World.player.actualHp <= 0)
                        plrDead(); // check for suicide
                    else
                    {
                        World.fight.renderFightToBuffer(ref fight);

                        World.ce.DrawConBuffer(fight);
                        World.ce.SwapBuffers();
                    }
                }
                else
                {
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
