using System;
using System.Collections.Generic;
using textured_raycast.maze.graphics;
using textured_raycast.maze.math;
using textured_raycast.maze.skills;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.texture;
using textured_raycast.maze.resources;

namespace textured_raycast.maze.Fight
{
    internal class Fight
    {
        string[] textures;
        int mapID = -1;

        Enemy sTF;

        public float tillFightBegins = 2;

        public float hp;
        public float maxHp;

        List<Sprite> sprites = new List<Sprite>();

        Vector2d startRot;
        Vector2d startPos;
        Vector2d startPlane;

        public Fight(Enemy spriteToFight)
        {
            sTF = spriteToFight;

            textures = sTF.texture;
            maxHp = sTF.hp;
            hp = maxHp;

            sprites.Add(new DefaultSprite(2.35, 2, textures));
        }

        public void enemyDoAction()
        {
            World.player.actualHp--;

            if (World.player.actualHp <= 0)
                plrDead();
        }

        public void enemyDead()
        {
            World.state = States.Game;

            World.player.xp++;

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
            buffer.DrawBox(60, 74, (int)(58 * (World.player.actualHp / World.player.hp)), 4, new TexColor(0, 255, 0));

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
