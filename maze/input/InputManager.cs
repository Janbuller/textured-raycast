namespace textured_raycast.maze.input
{
    enum KeyState {
        KEY_UP,
        KEY_DOWN,
        KEY_HELD,
    }

    class InputManager{

        static ILowLevelInput InputController;

        public static void SetInputController(ILowLevelInput Controller) {
            InputController = Controller;
            InputController.Init();
        }

        public static KeyState GetKey(Keys key) {
            KeyState returnKey = InputController.GetKey(key);
            if (returnKey == KeyState.KEY_DOWN)
            {
                World.currentMessage = "";
            }
            return returnKey;
        }
    }
}
