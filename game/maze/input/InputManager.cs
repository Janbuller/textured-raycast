using System.Collections.Generic;

namespace textured_raycast.maze.input
{
    enum KeyState
    {
        KEY_UP,
        KEY_DOWN,
        KEY_HELD,
    }

    class InputManager
    {

        static ILowLevelInput InputController;

        public static void SetInputController(ILowLevelInput Controller)
        {
            InputController = Controller;
            InputController.Init();
        }

        public static KeyState GetKey(Keys key)
        {
            KeyState returnKey = InputController.GetKey(key);
            if (returnKey == KeyState.KEY_DOWN)
            {
                World.currentMessage = "";
            }
            return returnKey;
        }

        public static Dictionary<KeyGroups, Keys[]> KeyGroup = new Dictionary<KeyGroups, Keys[]>()
        {
	    {KeyGroups.KG_UP,    new Keys[] {Keys.K_W, Keys.K_UP}},
	    {KeyGroups.KG_DOWN,  new Keys[] {Keys.K_S, Keys.K_DOWN}},
	    {KeyGroups.KG_LEFT,  new Keys[] {Keys.K_A, Keys.K_LEFT}},
	    {KeyGroups.KG_RIGHT, new Keys[] {Keys.K_D, Keys.K_RIGHT}},
        };

        public static KeyState GetKeyGroup(Keys[] keys)
        {
            KeyState EndState = KeyState.KEY_UP;

            foreach (Keys key in keys)
            {
                KeyState CurKeyState = GetKey(key);
                if (CurKeyState == KeyState.KEY_DOWN)
                {
                    EndState = KeyState.KEY_DOWN;
                    break;
                }
                else if (CurKeyState == KeyState.KEY_HELD)
                {
                    EndState = KeyState.KEY_HELD;
                }
            }

            return EndState;
        }

    }
}
