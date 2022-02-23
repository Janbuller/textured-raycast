using System.Collections.Generic;

namespace textured_raycast.maze.input
{
    class InputManager{

        static ILowLevelInput InputController;

        public static void SetInputController(ILowLevelInput Controller) {
            InputController = Controller;
            InputController.Init();
        }

        public static bool GetKey(Keys key) {
            return InputController.GetKey(key);
        }
    }
}
