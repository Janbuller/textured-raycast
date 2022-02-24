using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace textured_raycast.maze.input.console
{
    class Press {
        public bool Pressed;
        public long Time;

        public Press(bool Pressed, long Time) {
            this.Pressed = Pressed;
            this.Time = Time;
        }
    }
    class ConsoleInputController : ILowLevelInput {
        private Dictionary<Keys, Press> pressedKeys = new Dictionary<Keys, Press>() {};

        public bool GetKey(Keys key) {
            try {
                return pressedKeys[key].Pressed;
            } catch (KeyNotFoundException) {
                return false;
            }
        }

        public void Init() {
            Task.Run(() => { InputLoop(); });
        }

        private void InputLoop() {
            while(true) {
                while(Console.KeyAvailable) {
                    Keys key = ConvertKeys(Console.ReadKey().Key);

                    pressedKeys[key] = new Press(true, DateTime.Now.Ticks);
                }

                foreach(var item in pressedKeys) {
                    if(item.Value.Time + 2500000 < DateTime.Now.Ticks) {
                        pressedKeys[item.Key].Pressed = false;
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

                default:
                    return Keys.UNKNOWN;
            }
        }
    }
}
