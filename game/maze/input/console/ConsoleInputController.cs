using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace textured_raycast.maze.input.console
{
    class ConsoleInputController : ILowLevelInput {
        private Dictionary<Keys, Press> pressedKeys = new Dictionary<Keys, Press>() {};

	// Looks up the keystate in the pressedkeys dictionary. If the
	// key doesn't exist, it is up, if it is just down, it is set
	// to held and down is returned.
        public KeyState GetKey(Keys key) {
            try {
                if(pressedKeys[key].State == KeyState.KEY_DOWN) {
                    pressedKeys[key].State = KeyState.KEY_HELD;
                    return KeyState.KEY_DOWN;
                }
                return pressedKeys[key].State;
            } catch (KeyNotFoundException) {
                return KeyState.KEY_UP;
            } catch (NullReferenceException) {
                return KeyState.KEY_UP;
            }
        }

	// The init function runs the input loop in a thread.
        public void Init() {
            Task.Run(() => { InputLoop(); });
        }

	// Runs a loop, checking for input
        private void InputLoop() {
            while(true) {
		// Loops through each unpolled key.
                while(Console.KeyAvailable) {
		    // Gets the key as our own key enum
                    Keys key = ConvertKeys(Console.ReadKey(true).Key);

		    // Adds the key to the dictionary, setting the time to now.
                    pressedKeys[key] = new Press(KeyState.KEY_DOWN, DateTime.Now.Ticks);
                }

		// Loops through each key in the dictionary. If over
		// 2500000 ticks have passed, the key is set to up.
                foreach(var item in pressedKeys) {
                    if(item.Value.Time + 2500000 < DateTime.Now.Ticks) {
                        pressedKeys[item.Key].State = KeyState.KEY_UP;
                    }
                }
            }
        }

	// Converts ConsoleKey keys to our key enum.
        private Keys ConvertKeys(ConsoleKey key) {
            switch(key) {
                case ConsoleKey.W:
                    return Keys.K_W;
                case ConsoleKey.A:
                    return Keys.K_A;
                case ConsoleKey.S:
                    return Keys.K_S;
                case ConsoleKey.D:
                    return Keys.K_D;
                case ConsoleKey.E:
                    return Keys.K_E;

		case ConsoleKey.D1:
		    return Keys.K_1;
		case ConsoleKey.D2:
                    return Keys.K_2;
		case ConsoleKey.D3:
                    return Keys.K_3;

                case ConsoleKey.UpArrow:
                    return Keys.K_UP;
                case ConsoleKey.DownArrow:
                    return Keys.K_DOWN;
                case ConsoleKey.LeftArrow:
                    return Keys.K_LEFT;
                case ConsoleKey.RightArrow:
                    return Keys.K_RIGHT;

                case ConsoleKey.Escape:
                    return Keys.K_ESC;

                default:
                    return Keys.UNKNOWN;
            }
        }
    }
}
