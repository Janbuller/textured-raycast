using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.texture;

namespace textured_raycast.maze
{
    class ConsoleBuffer
    {
        int width, height;

        public List<TexColor> buffer;

        public int Width { get => width; set => width = value; }
        public int Height { get => height; set => height = value; }

        public ConsoleBuffer(int win_width, int win_height)
        {
            // Used to initialize the buffers to and empty sized list.
            TexColor[] tmp = new TexColor[win_width * win_height];
            buffer = tmp.ToList();

            // Sets game parameters.
            Width = win_width;
            Height = win_height;
        }

        public List<TexColor> getBuffer()
        {
            return buffer;
        }

        // Used to draw char to current buffer.
        public void DrawPixel(TexColor col, int x, int y)
        {
            // Return exception, if char is out of game window.
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return;
            }

            DrawToFramebuffer(col, x, y, ref buffer);
        }

        public TexColor GetPixel(int x, int y)
        {
            // Return exception, if char is out of game window.
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return new TexColor(0, 0, 0);
            }

            return buffer[x + y * Width];
        }

        // Places an overlay buffer over this one
        public ConsoleBuffer mixBuffer(ConsoleBuffer overlay)
        {
            ConsoleBuffer consoleBuffer = new ConsoleBuffer(Width, Height);

            List<TexColor> b1 = buffer;
            List<TexColor> b2 = overlay.getBuffer();


            for (int i = 0; i < buffer.Count; i++)
            {
                if (b2[i] is null)
                {
                    consoleBuffer.buffer[i] = b1[i];
                }
                else
                {
                    consoleBuffer.buffer[i] = b2[i];
                }
            }

            return consoleBuffer;
        }

        public void Clear()
        {
            TexColor[] tmp = new TexColor[Height * Width];
            buffer = tmp.ToList();
        }

        // Draws a centered vertical line, width xPos, Height and Color.
        public void DrawVerLine(int x, int height, TexColor color)
        {
            // Return exception, if char is out of game window.
            if (x < 0 || x > Width)
            {
                throw new ArgumentOutOfRangeException();
            }

            // Draw line, by choosing a starting Y and looping through height in
            // a for loop.
            int startY = Height / 2 - height / 2;
            for (int i = 0; i < height; i++)
            {
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                DrawPixel(color, x, startY + i);
            }
        }

        public void DrawVerLine(int x, int height, Texture tex, int texX, float darken, TexColor alphaCol = null)
        {
            // Return exception, if char is out of game window.
            if (x < 0 || x > Width)
            {
                throw new ArgumentOutOfRangeException();
            }

            int startY = Height / 2 - height / 2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - Height / 2 + height / 2) * sectionHeight;
            if (startY < 0)
            {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > Height ? Height : endY;
            for (int i = startY; i < endY; i++)
            {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                if (alphaCol == color)
                    continue;
                DrawPixel((color * darken), x, i);
            }
        }

        public void DrawVerLine(int x, int height, Texture tex, int texX, float darken, TexColor light, float mixBy, TexColor alphaCol = null)
        {
            // Return exception, if char is out of game window.
            if (x < 0 || x > Width)
            {
                throw new ArgumentOutOfRangeException();
            }

            int startY = Height / 2 - height / 2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - Height / 2 + height / 2) * sectionHeight;
            if (startY < 0)
            {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > Height ? Height : endY;
            for (int i = startY; i < endY; i++)
            {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                if (alphaCol == color)
                    continue;
                TexColor lightMul = TexColor.unitMultReal(color, light);

                // This doesn't work right now.
                // ============================
                //
                // double exposure = 1;
                // double gamma = 2.2;
                // lightMul.realR = (int)(Math.Pow((1.0f - Math.Exp(-lightMul.realR * exposure)), (1.0f/gamma)) * 255);
                // lightMul.realG = (int)(Math.Pow((1.0f - Math.Exp(-lightMul.realG * exposure)), (1.0f/gamma)) * 255);
                // lightMul.realB = (int)(Math.Pow((1.0f - Math.Exp(-lightMul.realB * exposure)), (1.0f/gamma)) * 255);
                //
                // Console.WriteLine(lightMul.realB);
                // Console.WriteLine((Math.Pow((1.0f - Math.Exp(-lightMul.realB * exposure)), (1.0f/gamma)) * 255));

                DrawPixel((color * darken * mixBy + lightMul * (1 - mixBy)), x, i);
            }
        }

        /// <summary>
        /// Draws a texture at a specific position.
        /// </summary>
        public void DrawTexture(Texture tex, int xP, int yP)
        {
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    DrawPixel(tex.getPixel(x, y), xP + x, yP + y);
                }
            }
        }

        /// <summary>
        /// Draws a texture at a specific position, ignoring any pixels of color <c>alpha</c>.
        /// </summary>
        public void DrawTexture(Texture tex, int xP, int yP, TexColor alpha)
        {
            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    TexColor pixel = tex.getPixel(x, y);
                    if (pixel != alpha)
                        DrawPixel(pixel, xP + x, yP + y);
                }
            }
        }

        // Draws a box.
        public void DrawBox(int xS, int yS, int w, int h, TexColor tc)
        {
            for (int x = xS; x < xS + w; x++)
                for (int y = yS; y < yS + h; y++)
                    DrawPixel(tc, x, y);
        }

        // Draws char to specific framebuffer. Used internally by DrawChar
        // functions.
        private void DrawToFramebuffer(TexColor col, int x, int y, ref List<TexColor> buffer)
        {
            buffer[x + y * Width] = col;
        }
    }
}
