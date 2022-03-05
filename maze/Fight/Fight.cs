using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;

namespace rpg_game.maze.Fight
{
    internal class Fight
    {
        int spriteID;
        int mapID = -1;

        public float tillFightBegins = 2;

        float hp;
        float maxHp;

        public Fight(Enemy spriteToFight)
        {
            spriteID = spriteToFight.spriteID;
            hp = spriteToFight.hp;
        }

        public void renderFightToBuffer(ref ConsoleBuffer buffer, ref World world)
        {
            Vector2d dir = new Vector2d(-1, 0);
            Vector2d pos = new Vector2d(3.65, 2);
            Vector2d plane = new Vector2d(0, 1) * 0.66;
            Map map = world.getMapByID(mapID);

            Maze.FloorCasting(ref buffer, dir, plane, pos, 1, map, world);

            double[] ZBuffer = new double[buffer.GetWinWidth()];

            Maze.WallCasting(ref buffer, ref ZBuffer, dir, plane, pos, 1, map);

            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(new DefaultSprite(2.35, 2, spriteID));

            Maze.SpriteCasting(ref buffer, sprites, pos, plane, dir, ZBuffer, 1);
        }

        public void renderFightStartScreenToBuffer(ref ConsoleBuffer buffer, float progress)
        {
            for (int x = 0; x < buffer.GetWinWidth(); x++)
            {
                for (int y = 0; y < buffer.GetWinHeight(); y++)
                {
                    if ((MathF.Atan2(buffer.GetWinWidth() / 2 - y, buffer.GetWinHeight() / 2 - x)/MathF.PI+1)/2 > progress)
                    {
                        buffer.DrawPixel(new textured_raycast.maze.texture.TexColor(0, 0, 0), x, y);
                    }
                }
            }
        }
    }
}
