using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;
using System.Drawing;
using textured_raycast.maze.texture;

namespace textured_raycast.maze
{
    class ConsoleBuffer {
        int width, height;

        List<TexColor> buffer;

        public ConsoleBuffer(int win_width, int win_height) {
            // Used to initialize the buffers to and empty sized list.
            TexColor[] tmp = new TexColor[win_width * win_height];
            buffer = tmp.ToList();

            // Sets game parameters.
            width = win_width;
            height = win_height;
        }

        public int GetWinWidth() { return width; }
        public int GetWinHeight() { return height; }

        public List<TexColor> getBuffer() {
            return buffer;
        }

        // Used to draw char to current buffer.
        public void DrawPixel(TexColor col, int x, int y) {
            // Return exception, if char is out of game window.
            if (x < 0 || x >= GetWinWidth() || y < 0 || y >= GetWinHeight()) {
                return;
            }

            DrawToFramebuffer(col, x, y, ref buffer);
        }

        // Places an overlay buffer over this one
        public ConsoleBuffer mixBuffer(ConsoleBuffer overlay)
        {
            ConsoleBuffer consoleBuffer = new ConsoleBuffer(width, height);

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

        public void Clear() {
            TexColor[] tmp = new TexColor[GetWinHeight() * GetWinWidth()];
            buffer = tmp.ToList();
        }

        // Draws a centered vertical line, width xPos, Height and Color.
        public void DrawVerLine(int x, int height, TexColor color) {
            // Return exception, if char is out of game window.
            if (x < 0 || x > GetWinWidth()) {
                throw new ArgumentOutOfRangeException();
            }

            // Draw line, by choosing a starting Y and looping through height in
            // a for loop.
            int startY = GetWinHeight()/2 - height/2;
            for(int i = 0; i < height; i++) {
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                DrawPixel(color, x, startY+i);
            }
        }

        public void DrawVerLine(int x, int height, Texture tex, int texX, float darken, TexColor alphaCol = null) {
            // Return exception, if char is out of game window.
            if (x < 0 || x > GetWinWidth()) {
                throw new ArgumentOutOfRangeException();
            }

            int startY = GetWinHeight()/2 - height/2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - GetWinHeight() / 2 + height /2) * sectionHeight;
            if(startY < 0) {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > GetWinHeight() ? GetWinHeight() : endY;
            for(int i = startY; i < endY; i++) {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                if(alphaCol == color)
                    continue;
                DrawPixel((color * darken), x, i);
            }
        }

        public void DrawVerLine(int x, int height, Texture tex, int texX, float darken, TexColor light, TexColor alphaCol = null) {
            // Return exception, if char is out of game window.
            if (x < 0 || x > GetWinWidth()) {
                throw new ArgumentOutOfRangeException();
            }

            int startY = GetWinHeight()/2 - height/2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - GetWinHeight() / 2 + height /2) * sectionHeight;
            if(startY < 0) {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > GetWinHeight() ? GetWinHeight() : endY;
            for(int i = startY; i < endY; i++) {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                if(alphaCol == color)
                    continue;
                const float mixBy = 0.7f;
                DrawPixel((color * darken * mixBy + TexColor.unitMultReal(color, light) * (1 - mixBy)), x, i);
            }
        }

        /// <summary>
        /// Draws a texture at a specific position.
        /// </summary>
        public void DrawTexture(Texture tex, int xP, int yP) {
            for(int y = 0; y < tex.height; y++) {
                for(int x = 0; x < tex.width; x++) {
                    DrawPixel(tex.getPixel(x, y), xP+x, yP+y);
                }
            }
        }

        /// <summary>
        /// Draws a texture at a specific position, ignoring any pixels of color <c>alpha</c>.
        /// </summary>
        public void DrawTexture(Texture tex, int xP, int yP, TexColor alpha) {
            for(int y = 0; y < tex.height; y++) {
                for(int x = 0; x < tex.width; x++) {
                    TexColor pixel = tex.getPixel(x, y);
                    if(pixel != alpha)
                        DrawPixel(pixel, xP+x, yP+y);
                }
            }
        }

        // Draws char to specific framebuffer. Used internally by DrawChar
        // functions.
        private void DrawToFramebuffer(TexColor col, int x, int y, ref List<TexColor> buffer) {
            buffer[x + y*GetWinWidth()] = col;
        }
    }
}
