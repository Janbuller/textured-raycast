using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace textured_raycast.maze.input.Windows
{
    // this uses Keyboard.cs
    // wich is no by us, bu we did write this all ourselves
    class WindowsInputController : ILowLevelInput {
        // make a list of all keys and their current state
        private Dictionary<input.Keys, KeyState> pressedKeys = new Dictionary<input.Keys, KeyState>() {};
        
        // make a list to convert the keys from keyboard.cs to the keys we use
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
            // when the player wants to get a key
            try
            {
                // check if the key is pressed (has been pressed this frame)
                if (pressedKeys[key] == KeyState.KEY_DOWN)
                {
                    // if it is held, it will return down next frame
                    pressedKeys[key] = KeyState.KEY_HELD;
                    // return down
                    return KeyState.KEY_DOWN;
                }

                // return the keystate
                return pressedKeys[key];
            }
            catch (KeyNotFoundException)
            {
                // if the key is not found, it is neither held nor pressed
                // so up
                return KeyState.KEY_UP;
            }
            catch (NullReferenceException)
            {
                // same with nullReferenceExeption
                return KeyState.KEY_UP;
            }
        }

        public void Init()
        {
            Task.Run(() => { InputLoop(); }); // run a task with the input loop, more or less like a thread
        }

        private void InputLoop()
        {
            while (true) // forever, because its a different thread/task
            {
                // go thru all keys we use
                foreach (KeyValuePair<input.Keys, Keys> keyValuePair in converIKeysToKeys)
                {
                    // if the key dose not exist
                    if (!pressedKeys.ContainsKey(keyValuePair.Key))
                    {
                        // create it
                        pressedKeys[keyValuePair.Key] = KeyState.KEY_UP;
                    }

                    // if the key is pressed (form keyboard that we did not write)
                    if (Keyboard.IsKeyPressed(keyValuePair.Value))
                    {
                        // and key was not pressed before, it becomes down
                        if (pressedKeys[keyValuePair.Key] == KeyState.KEY_UP)
                        {
                            pressedKeys[keyValuePair.Key] = KeyState.KEY_DOWN;
                        }

                        // that means that if it is held it stays held
                    }
                    else
                    {
                        // if it is not pressed then it becomes up
                        pressedKeys[keyValuePair.Key] = KeyState.KEY_UP;
                    }
                }
            }
        }
    }
}
