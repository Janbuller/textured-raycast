using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Pastel;
using System.Drawing;
using textured_raycast.maze.texture;

namespace textured_raycast.maze
{
    class MazeEngine {
        GameParams parameters = new GameParams();

        // Double buffer system made for futureproofing. Allows for possible
        // multi-threading. One thread rendering, another running the game.
        // One buffer is show, while the game thread renders to the other.
        List<TexColor> buffer1;
        List<TexColor> buffer2;

        bool firstBuffer = true;

        public MazeEngine(int win_width, int win_height, string game_name) {
            // Used to initialize the buffers to and empty sized list.
            TexColor[] tmp = new TexColor[win_width * win_height];
            buffer1 = tmp.ToList();
            buffer2 = tmp.ToList();
            // Sets game parameters.
            parameters.winWidth = win_width;
            parameters.winHeight = win_height;
            parameters.name = game_name;

            // Sets the console window title to the game name.
            Console.Title = game_name;
        }

        public int GetWinWidth() { return parameters.winWidth; }
        public int GetWinHeight() { return parameters.winHeight; }

        // Used to draw char to current buffer.
        public void DrawChar(TexColor col, int x, int y) {
            // Return exception, if char is out of game window.
            if (x < 0 || x >= GetWinWidth() || y < 0 || y >= GetWinHeight()) {
                return;
            }

            // Decides buffer to draw to. Does so using other function.
            if(!firstBuffer) {
                DrawToFramebuffer(col, x, y, ref buffer1);
            } else {
                DrawToFramebuffer(col, x, y, ref buffer2);
            }
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
                DrawChar(color, x, startY+i);
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
                DrawChar((color * darken), x, i);
            }

            // int startY = GetWinHeight()/2 - height/2;
            // int secNum = 0;
            // for(int i = 0; i < height; i+=sectionHeight) {
            //     // Draw the line, using NuGet package "Pastel" to color, using
            //     // ansi escape sequences.
            //     for(int j = 0; j < sectionHeight; j++) {
            //         DrawChar("█".Pastel(allSecCol[i-(secNum*sectionHeight)].getSysColor()), x, startY+i+j);
            //     }
            //     secNum++;
            // }
        }

        /// <summary>
        /// Draws a texture at a specific position.
        /// </summary>
        public void DrawTexture(Texture tex, int xP, int yP) {
            for(int y = 0; y < tex.height; y++) {
                for(int x = 0; x < tex.width; x++) {
                    DrawChar(tex.getPixel(x, y), xP+x, yP+y);
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
                        DrawChar(pixel, xP+x, yP+y);
                }
            }
        }

        // Draws a border around game window
        public void DrawBorder() {
            int winWidth = GetWinWidth();
            int winHeight = GetWinHeight();
            TexColor whiteColor = new TexColor(255, 255, 255);
            for (int y = 0; y <  winHeight; y++) {
                DrawChar(whiteColor, 0, y);
                DrawChar(whiteColor, winWidth-1, y);
            }
            for(int x = 0; x < winWidth; x++) {
                DrawChar(whiteColor, x, 0);
                DrawChar(whiteColor, x, winHeight-1);
            }
        }

        // Draws char to specific framebuffer. Used internally by DrawChar
        // functions.
        private void DrawToFramebuffer(TexColor col, int x, int y, ref List<TexColor> buffer) {
            buffer[x + y*GetWinWidth()] = col;
        }

        // Swaps the buffers.
        public void SwapBuffers() {
            TexColor[] tmp = new TexColor[GetWinWidth() * GetWinHeight()];

            // Clears the buffer, to be rendered to now.
            if(firstBuffer) {
                buffer1 = tmp.ToList();
            } else {
                buffer2 = tmp.ToList();
            }

            firstBuffer = !firstBuffer;
        }

        // Draws the active buffer, using DrawBuffer.
        public void DrawScreen() {
            if(firstBuffer) {
                DrawBuffer(buffer1);
            } else {
                DrawBuffer(buffer2);
            }
        }

        // Draws specific buffer.
        private void DrawBuffer(List<TexColor> buffer) {
            int winWidth = GetWinWidth();
            int winHeight = GetWinHeight();

            Console.CursorVisible = false;
            // Move the cursor to the top, rendering over the previous screen.
            // This used to be Console.Clear(), but that has insane flickering
            // problems on Windows.
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            // Draw buffer, line by line. This improves performance.
            for (int y = 0; y <  winHeight; y+=2) {
                string line = "";
                for(int x = 0; x < winWidth; x++) {
                    line += "▀".Pastel(buffer[x + y*winWidth].getSysColor()).PastelBg(buffer[x + (y+1) * winWidth].getSysColor());
                    if(buffer[x + y*winWidth] is null) {
                        line += " ".PastelBg(Color.Black);
                    }
                }
                Console.WriteLine(line);
            }
        }
    }
    class GameParams {
        public int winWidth, winHeight;
        public string name;
    }
}
