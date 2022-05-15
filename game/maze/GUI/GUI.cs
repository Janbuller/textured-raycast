using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using textured_raycast.maze.input;
using textured_raycast.maze.resources;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.GUI
{
    internal class GUI
    {
        // all the differen images
        // 1-3 are the differen sizes of chat boxes
        // 4-6 are the menus (selected 1 2 or 3)
        static Dictionary<int, string> GUITextures = new Dictionary<int, string>() {
            {1,   "img/gui/chatBox1.ppm"},
            {2,   "img/gui/chatBox2.ppm"},
            {3,   "img/gui/chatBox3.ppm"},
            {4,   "img/gui/Menu1.ppm"},
            {5,   "img/gui/Menu2.ppm"},
            {6,   "img/gui/Menu3.ppm"},
        };

        // set of what characters represent what image
        static string GUITextStrings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        // the image with all characters
        static List<Texture> GUIText = SpriteSheetLoaders.loadHorisontalSpriteSheetFromPlainPPM("img/gui/Font.ppm", 3);

        // the curent selexted option
        public static int pauseUIIndex = 1;

        public static void pauseGUI(ref ConsoleBuffer guiBuffer)
        {
            // draw the current image based on ui idndex
            guiBuffer.DrawTexture(ResourceManager.getTexture(GUITextures[pauseUIIndex + 3]), 10, 10, new TexColor(0, 0, 0));

            // register input
            HandleInputUI();
        }
        
        public static void text(ref ConsoleBuffer guiBuffer, string toSend, int x, int y, int w)
        { // Draw text function

            // where the text starts
            Vector2i start = new Vector2i(x, y);

            // repeat until there is not text left
            while (toSend.Length > 0)
            {
                // this GUITextStrings.IndexOf(toSend.Substring(0, 1).ToUpper())
                // gets the index of the current char, in the masterstring

                // draw a character at the x y
                addCharToBufferAt(ref guiBuffer, GUITextStrings.IndexOf(toSend.Substring(0, 1).ToUpper()), start.X, start.Y);

                // remove the first index of the string
                toSend = toSend.Remove(0, 1);
                
                // add 4, a char is 3 pixles long
                start.X += 4;
                if (start.X > x+w)
                {
                    // if it is higher that width + what it starts with
                    // make it loop
                    start.X = x;
                    start.Y += 6;
                }
            }
        }

        public static void texBox(ref ConsoleBuffer guiBuffer, string toSend)
        {
            // start position
            Vector2i start = new Vector2i(0, 0);

            // the image to use
            int texToUseIdx = 0;

            // chose what box to use based on the tet length
            if (toSend.Length < 24)
            {
                texToUseIdx = 1;
            }
            else if (toSend.Length < 48)
            {
                texToUseIdx = 2;
            }
            else
            {
                texToUseIdx = 3;
            }

            // get and draw the image
            Texture texToUse = ResourceManager.getTexture(GUITextures[texToUseIdx]);
            guiBuffer.DrawTexture(texToUse, 1, guiBuffer.Height - texToUse.height - 1);

            // set x and y of start
            start = new Vector2i(3, guiBuffer.Height - texToUse.height + 1);

            // same loop as text function /\
            while (toSend.Length > 0)
            {
                addCharToBufferAt(ref guiBuffer, GUITextStrings.IndexOf(toSend.Substring(0, 1).ToUpper()), start.X, start.Y);
                toSend = toSend.Remove(0, 1);
                start.X += 4;
                if (start.X > 98)
                {
                    start.X = 3;
                    start.Y += 6;
                }
            }
        }

        public static void addCharToBufferAt(ref ConsoleBuffer UIHolder, int charPosition, int x, int y)
        {
            if (charPosition != -1)
                // get the texture of the image at the position, and draw it at x y
                UIHolder.DrawTexture(GUIText[charPosition], x, y, new TexColor(0, 0, 0));
        }

        public static void HandleInputUI()
        {
            // if you press e
            if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
            {
                // swap the state based on what uiindex you have
                switch (pauseUIIndex)
                {
                    case 1:
                        World.state = States.Inventory;
                        break;
                    case 2:
                        World.state = States.Settings; // there are no settings, this just changes to / from console mode
                        break;
                    case 3:
                        World.state = States.Stopping;
                        break;
                }
            }
            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                // escape to go back to game
                World.state = States.Game;
            }

            // up goes up, down goes down
            if (InputManager.GetKey(Keys.K_UP) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_W) == KeyState.KEY_DOWN)
            {
                pauseUIIndex = Math.Max(1, pauseUIIndex - 1);
            }
            if (InputManager.GetKey(Keys.K_DOWN) == KeyState.KEY_DOWN || InputManager.GetKey(Keys.K_S) == KeyState.KEY_DOWN)
            {
                pauseUIIndex = Math.Min(3, pauseUIIndex + 1);
            }
        }
    }
}
