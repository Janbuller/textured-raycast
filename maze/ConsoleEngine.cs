using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;
using System.Drawing;
using textured_raycast.maze.texture;

namespace textured_raycast.maze
{
    struct GameParams
    {
        public int winWidth, winHeight;
        public string name;
    }

    class ConsoleEngine
    {
        GameParams parameters = new GameParams();

        // Double buffer system made for futureproofing. Allows for possible
        // multi-threading. One thread rendering, another running the game.
        // One buffer is show, while the game thread renders to the other.
        List<TexColor> buffer1;
        List<TexColor> buffer2;

        ConsoleBuffer curConBuf;

        bool firstBuffer = true;

        public int Width { get => parameters.winWidth; set => parameters.winWidth = value; }
        public int Height { get => parameters.winHeight; set => parameters.winHeight = value; }

        public ConsoleEngine(int win_width, int win_height, string game_name)
        {
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

        public void DrawConBuffer(ConsoleBuffer buf)
        {
            curConBuf = buf;
            if (firstBuffer)
            {
                buffer2 = buf.getBuffer();
            }
            else
            {
                buffer1 = buf.getBuffer();
            }
        }

	public ConsoleBuffer GetCurrentBuffer() {
            return curConBuf;
        }

        // Swaps the buffers.
        public void SwapBuffers()
        {
            TexColor[] tmp = new TexColor[Width * Height];

            // Clears the buffer, to be rendered to now.
            if (firstBuffer)
            {
                buffer1 = tmp.ToList();
            }
            else
            {
                buffer2 = tmp.ToList();
            }

            firstBuffer = !firstBuffer;
        }

        // Draws the active buffer, using DrawBuffer.
        public void DrawScreen()
        {
            if (firstBuffer)
            {
                DrawBuffer(buffer1);
            }
            else
            {
                DrawBuffer(buffer2);
            }
        }

        // Draws specific buffer.
        private void DrawBuffer(List<TexColor> buffer)
        {
            int winWidth = Width;
            int winHeight = Height;

            Console.CursorVisible = false;
            // Move the cursor to the top, rendering over the previous screen.
            // This used to be Console.Clear(), but that has insane flickering
            // problems on Windows.
            Console.CursorTop = 0;
            // Draw buffer, line by line. This improves performance.
            for (int y = 0; y < winHeight; y += 2)
            {
                string line = "";
                for (int x = 0; x < winWidth; x++)
                {
                    if (buffer[x + y * winWidth] is null || buffer[x + (y + 1) * winWidth] is null)
                    {
                        line += " ".PastelBg(Color.Black);
                    }
                    else
                    {
                        line += "▀".Pastel(buffer[x + y * winWidth].getSysColor()).PastelBg(buffer[x + (y + 1) * winWidth].getSysColor());
                    }
                }
                // This makes every line render at the far left. This is
                // necessary, due to the asynchronous input handling, when using
                // console-based input, as it shows the pressed button, moving
                // the rendered line to the right.
                Console.CursorLeft = 0;
                Console.WriteLine(line);
            }
        }
    }
}
