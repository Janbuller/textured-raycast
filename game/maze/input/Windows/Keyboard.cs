using System.ComponentModel;
using System.Runtime.InteropServices;

namespace textured_raycast.maze.input.Windows
{
    // this was taken from
    // https://www.reddit.com/r/csharp/comments/fskrz4/how_to_detect_different_key_presses_at_the_same/fm2w7te/?utm_source=reddit&utm_medium=web2x&context=3
    // and slightly modified to suit our needs

    public static class Keyboard
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetKeyboardState(byte[] lpKeyState);

        public static int GetKeyState()
        {


            byte[] keys = new byte[256];

            //Get pressed keys
            if (!GetKeyboardState(keys))
            {
                int err = Marshal.GetLastWin32Error();
                throw new Win32Exception(err);
            }

            for (int i = 0; i < 256; i++)
            {

                byte key = keys[i];

                //Logical 'and' so we can drop the low-order bit for toggled keys, else that key will appear with the value 1!
                if ((key & 0x80) != 0)
                {

                    //This is just for a short demo, you may want this to return
                    //multiple keys!
                    return (int)key;
                }
            }
            return -1;
        }

        [DllImport("user32.dll")]
        static extern short GetKeyState(Keys nVirtKey);

        public static bool IsKeyPressed(Keys testKey)
        {
            bool keyPressed = false;
            short result = GetKeyState(testKey);

            switch (result)
            {
                case 0:
                    // Not pressed and not toggled on.
                    keyPressed = false;
                    break;

                case 1:
                    // Not pressed, but toggled on
                    keyPressed = false;
                    break;

                default:
                    // Pressed (and may be toggled on)
                    keyPressed = true;
                    break;
            }

            return keyPressed;
        }



        private const uint MAPVK_VK_TO_CHAR = 2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        static extern uint MapVirtualKeyW(uint uCode, uint uMapType);

        public static char KeyToChar(Keys key)
        {
            return unchecked((char)MapVirtualKeyW((uint)key, MAPVK_VK_TO_CHAR)); // Ignore high word.  
        }
    }

    public enum Keys : short
    {
        K_W = 0x57,
        K_A = 0x41,
        K_S = 0x53,
        K_D = 0x44,
        K_E = 0x45,

        K_1 = 0x31,
        K_2 = 0x32,
        K_3 = 0x33,

        K_SHIFT = 0x10,

        K_UP = 0x26,
        K_DOWN = 0x28,
        K_LEFT = 0x25,
        K_RIGHT = 0x27,

        K_ESC = 0x1b,
    }
}
