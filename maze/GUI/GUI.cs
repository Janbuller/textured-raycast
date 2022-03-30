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
        static Dictionary<int, string> GUITextures = new Dictionary<int, string>() {
            {1,   "img/gui/chatBox1.ppm"},
            {2,   "img/gui/chatBox2.ppm"},
            {3,   "img/gui/chatBox3.ppm"},
            {4,   "img/gui/Menu1.ppm"},
            {5,   "img/gui/Menu2.ppm"},
            {6,   "img/gui/Menu3.ppm"},
        };

        static string GUITextStrings = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        static List<Texture> GUIText = SpriteSheetLoaders.loadHorisontalSpriteSheetFromPlainPPM("img/gui/Font.ppm", 3);

        public static int pauseUIIndex = 1;

        public static void pauseGUI(ref ConsoleBuffer guiBuffer)
        {
            guiBuffer.DrawTexture(ResourceManager.getTexture(GUITextures[pauseUIIndex + 3]), 10, 10, new TexColor(0, 0, 0));

            HandleInputUI();
        }
        public static void text(ref ConsoleBuffer guiBuffer, string toSend, int x, int y, int w)
        {
            Vector2i start = new Vector2i(x, y);

            while (toSend.Length > 0)
            {
                addCharToBufferAt(ref guiBuffer, GUITextStrings.IndexOf(toSend.Substring(0, 1).ToUpper()), start.x, start.y);
                toSend = toSend.Remove(0, 1);
                start.x += 4;
                if (start.x > x+w)
                {
                    start.x = x;
                    start.y += 6;
                }
            }
        }

        public static void texBox(ref ConsoleBuffer guiBuffer, string toSend)
        {
            Vector2i start = new Vector2i(0, 0);
            int texToUseIdx = 0;

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

            Texture texToUse = ResourceManager.getTexture(GUITextures[texToUseIdx]);
            guiBuffer.DrawTexture(texToUse, 1, guiBuffer.Height - texToUse.height - 1);
            start = new Vector2i(3, guiBuffer.Height - texToUse.height + 1);

            while (toSend.Length > 0)
            {
                addCharToBufferAt(ref guiBuffer, GUITextStrings.IndexOf(toSend.Substring(0, 1).ToUpper()), start.x, start.y);
                toSend = toSend.Remove(0, 1);
                start.x += 4;
                if (start.x > 98)
                {
                    start.x = 3;
                    start.y += 6;
                }
            }
        }

        public static void addCharToBufferAt(ref ConsoleBuffer UIHolder, int charPosition, int x, int y)
        {
            if (charPosition != -1)
                UIHolder.DrawTexture(GUIText[charPosition], x, y, new TexColor(0, 0, 0));
        }

        public static void HandleInputUI()
        {
            if (InputManager.GetKey(Keys.K_E) == KeyState.KEY_DOWN)
            {
                switch (pauseUIIndex)
                {
                    case 1:
                        World.state = states.Inventory;
                        break;
                    case 2:
                        World.state = states.Settings;
                        break;
                    case 3:
                        World.state = states.Stopping;
                        break;
                }
            }
            if (InputManager.GetKey(Keys.K_ESC) == KeyState.KEY_DOWN)
            {
                World.state = states.Game;
            }
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
