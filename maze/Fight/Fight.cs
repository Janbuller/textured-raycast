using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.graphics;
using textured_raycast.maze.texture;
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

        List<Sprite> sprites = new List<Sprite>();

        public Fight(Enemy spriteToFight)
        {
            spriteID = spriteToFight.spriteID;
            hp = spriteToFight.hp;

            sprites.Add(new DefaultSprite(2.35, 2, spriteID));

        }

        public void renderFightToBuffer(ref ConsoleBuffer buffer, ref World world)
        {
            Vector2d dir = new Vector2d(-1, 0);
            Vector2d pos = new Vector2d(3.65, 2);
            Vector2d plane = new Vector2d(0, 1) * 0.66;
            Map map = world.getMapByID(mapID);

            Texture FloorAndRoof = new Texture(buffer.Width, buffer.Height);
            FloorCasting.FloorCast(ref FloorAndRoof, dir, plane, pos, 1, map, world, world.textures);
            buffer.DrawTexture(FloorAndRoof, 0, 0);

            double[] ZBuffer = new double[buffer.Width];

            WallCasting.WallCast(ref buffer, ref ZBuffer, dir, plane, pos, 1, map, world.textures);

            foreach (Sprite sprite in sprites)
            {
                sprite.updateAnimation(world.dt);
            }

            SpriteCasting.SpriteCast(ref buffer, sprites, pos, plane, dir, ZBuffer, 1, map, ref world);
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
