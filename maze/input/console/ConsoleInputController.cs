using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace textured_raycast.maze.input.console
{
    class Press {
        public KeyState State;
        public long Time;

        public Press(KeyState State, long Time) {
            this.State = State;
            this.Time = Time;
        }
    }

    class ConsoleInputController : ILowLevelInput {
        private Dictionary<Keys, Press> pressedKeys = new Dictionary<Keys, Press>() {};

        public KeyState GetKey(Keys key) {
            try {
                if(pressedKeys[key].State == KeyState.KEY_DOWN) {
                    pressedKeys[key].State = KeyState.KEY_HELD;
                    return KeyState.KEY_DOWN;
                }
                return pressedKeys[key].State;
            } catch (KeyNotFoundException) {
                return KeyState.KEY_UP;
            }
        }

        public void Init() {
            Task.Run(() => { InputLoop(); });
        }

        private void InputLoop() {
            while(true) {
                while(Console.KeyAvailable) {
                    Keys key = ConvertKeys(Console.ReadKey(true).Key);

                    pressedKeys[key] = new Press(KeyState.KEY_DOWN, DateTime.Now.Ticks);
                }

                foreach(var item in pressedKeys) {
                    if(item.Value.Time + 2500000 < DateTime.Now.Ticks) {
                        pressedKeys[item.Key].State = KeyState.KEY_UP;
                    }
                }
            }
        }

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
