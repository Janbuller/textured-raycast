using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using textured_raycast.maze.input.Windows;

namespace textured_raycast.maze.input.Windows
{
    class WindowsInputController : ILowLevelInput {
        private Dictionary<input.Keys, KeyState> pressedKeys = new Dictionary<input.Keys, KeyState>() {};
        private Dictionary<input.Keys, Keys> converIKeysToKeys = new Dictionary<input.Keys, Keys>()
        {
            { input.Keys.K_A, Keys.K_A},
            { input.Keys.K_W, Keys.K_W},
            { input.Keys.K_S, Keys.K_S},
            { input.Keys.K_D, Keys.K_D},
            { input.Keys.K_E, Keys.K_E},

            { input.Keys.K_1, Keys.K_1},
            { input.Keys.K_2, Keys.K_2},
            { input.Keys.K_3, Keys.K_3},

            { input.Keys.K_SHIFT, Keys.K_SHIFT},

            { input.Keys.K_UP, Keys.K_UP},
            { input.Keys.K_DOWN, Keys.K_DOWN},
            { input.Keys.K_LEFT, Keys.K_LEFT},
            { input.Keys.K_RIGHT, Keys.K_RIGHT},

            { input.Keys.K_ESC, Keys.K_ESC},
        };

        public KeyState GetKey(input.Keys key)
        {
            try
            {
                if (pressedKeys[key] == KeyState.KEY_DOWN)
                {
                    pressedKeys[key] = KeyState.KEY_HELD;
                    return KeyState.KEY_DOWN;
                }
                return pressedKeys[key];
            }
            catch (KeyNotFoundException)
            {
                return KeyState.KEY_UP;
            }
            catch (NullReferenceException)
            {
                return KeyState.KEY_UP;
            }
        }

        public void Init()
        {
            Task.Run(() => { InputLoop(); });
        }

        private void InputLoop()
        {
            while (true)
            {
                foreach (KeyValuePair<input.Keys, Keys> keyValuePair in converIKeysToKeys)
                {
                    if (!pressedKeys.ContainsKey(keyValuePair.Key))
                    {
                        pressedKeys[keyValuePair.Key] = KeyState.KEY_UP;
                    }

                    if (Keyboard.IsKeyPressed(keyValuePair.Value))
                    {
                        if (pressedKeys[keyValuePair.Key] == KeyState.KEY_UP)
                        {
                            pressedKeys[keyValuePair.Key] = KeyState.KEY_DOWN;
                        }
                    }
                    else
                    {
                        if (pressedKeys[keyValuePair.Key] != KeyState.KEY_DOWN)
                        {
                            pressedKeys[keyValuePair.Key] = KeyState.KEY_UP;
                        }
                    }
                }
            }
        }
    }
}
