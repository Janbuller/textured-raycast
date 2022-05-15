using System.Collections.Generic;

namespace textured_raycast.maze.input
{
    class InputManager
    {
	// The actual inputcontroller to be used for lowlevel input
        static ILowLevelInput InputController;

	// Sets the global, static inputcontroller.
        public static void SetInputController(ILowLevelInput Controller)
        {
            InputController = Controller;
            InputController.Init();
        }

	// Polls the inputcontroller for a key and removes the current
	// message from the world.
        public static KeyState GetKey(Keys key)
        {
            KeyState returnKey = InputController.GetKey(key);
            if (returnKey == KeyState.KEY_DOWN)
            {
                World.currentMessage = "";
            }
            return returnKey;
        }

	// All the different keygroups.
        public static Dictionary<KeyGroups, Keys[]> KeyGroup = new Dictionary<KeyGroups, Keys[]>()
        {
        {KeyGroups.KG_UP,    new Keys[] {Keys.K_W, Keys.K_UP}},
        {KeyGroups.KG_DOWN,  new Keys[] {Keys.K_S, Keys.K_DOWN}},
        {KeyGroups.KG_LEFT,  new Keys[] {Keys.K_A, Keys.K_LEFT}},
        {KeyGroups.KG_RIGHT, new Keys[] {Keys.K_D, Keys.K_RIGHT}},
        };

	// Polls the inputcontroller for all keys in a keygroup. The
	// state will be up, unless one was just pressed, then down,
	// otherwise if one is held, it will be held.
        public static KeyState GetKeyGroup(Keys[] keys)
        {
	    // Start by setting the default keystate.
            KeyState EndState = KeyState.KEY_UP;

	    // Loop through all keys in a group.
            foreach (Keys key in keys)
            {
		// Poll for the keystate.
                KeyState CurKeyState = GetKey(key);

		// If the key was just pressed, break the loop and
		// return that.
                if (CurKeyState == KeyState.KEY_DOWN)
                {
                    EndState = KeyState.KEY_DOWN;
                    break;
                }
		// If the key is being held, set the endstate to that
		// and keep looping, in case any other key was just
		// pressed.
                else if (CurKeyState == KeyState.KEY_HELD)
                {
                    EndState = KeyState.KEY_HELD;
                }
            }

            return EndState;
        }

    }
}
