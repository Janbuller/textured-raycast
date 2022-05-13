using System;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.graphics
{
    class Skybox
    {

        // Draws the skybox to the top half of the game screen. This isn't very
        // optimized, and shouldn't be used, as it draws to pixels, that will
        // later be drawn over.
        public static void DrawSkybox(ref ConsoleBuffer game, Texture skyboxTex) {
            int winHeight = game.Height;

            for(int y = 0; y < game.Height / 2; y++) {
                for(int x = 0; x < game.Width; x++) {
                    var pix = GetSkyboxPixel(winHeight, skyboxTex, x, y, World.dayTime);
                    game.DrawPixel(pix, x, y);
                }
            }
        }

        public static TexColor GetSkyboxPixel(int winHeight, Texture skyboxTex, int x, int y, float dayTime)
        {
            // The difference between the height of on pixel on the screen and
            // on the texture, were the texture to fill the top helf of the
            // screen.
            double heightDiff = skyboxTex.height / (winHeight * 0.5);
            // Calibrated pixel position. This is the pixel on the texture,
            // closest to the one on the screen at x and y.
            Vector2i cal = new Vector2i((int)(x * heightDiff), (int)(y * heightDiff));

            // Get the players rotation in radians.
            double dirRad = Math.Atan2(World.plrRot.X, World.plrRot.Y);
            // The players rotation, mapped from the range [0, 2PI] to [0, 1].
            double dirMapped = dirRad / (Math.PI * 2);
            // The offset to the skybox texture, based of the mapped direction.
            int xOffset = (int)(dirMapped * skyboxTex.width);

            // Add the offset to the position.
            cal.X += xOffset;

            // While the position is outside the texture width, move back by the
            // amount over the bounds. This makes the texture repeating on the
            // x-axis.
            while (cal.X > skyboxTex.width)
                cal.X -= skyboxTex.width;
            while (cal.X < 0)
                cal.X += skyboxTex.width;

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
            return skyboxTex.getPixel(cal.X, cal.Y);


        }

    }
}
