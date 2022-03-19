using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace textured_raycast.maze.input.linux
{
    class LinuxInputController : ILowLevelInput {
        private Dictionary<Keys, KeyState> pressedKeys = new Dictionary<Keys, KeyState>() {};

        private const int bufLen = 24;
        private byte[] evBuf = new byte[bufLen];

        private FileStream evStream;

        public KeyState GetKey(Keys key) {
            try {
                if(pressedKeys[key]== KeyState.KEY_DOWN) {
                    pressedKeys[key]= KeyState.KEY_HELD;
                    return KeyState.KEY_DOWN;
                }
                return pressedKeys[key];
            } catch (KeyNotFoundException) {
                return KeyState.KEY_UP;
            }
        }

        public void Init() {
            evStream = new FileStream("/dev/input/event3", FileMode.Open, FileAccess.Read);
            Task.Run(() => { InputLoop(); });
        }

        private void InputLoop() {
            while(true) {
                evStream.Read(evBuf, 0, bufLen);
                types  type  = (types)BitConverter.ToInt16(evBuf, 16);
                codes  code  = (codes)BitConverter.ToInt16(evBuf, 18);
                values value = (values)BitConverter.ToInt32(evBuf, 20);
                Keys convCode = (Keys)Enum.Parse(typeof(Keys), code.ToString());

                switch(type) {
                    case types.EV_KEY:
                        switch(value) {
                            case values.KEY_DOWN:
                                pressedKeys[convCode] = KeyState.KEY_DOWN;
                                break;
                            case values.KEY_UP:
                                pressedKeys[convCode] = KeyState.KEY_UP;
                                break;
                        }
                        break;
                }
            }
        }
    }

    public enum codes : short {
        K_W = 17,
        K_A = 30,
        K_S = 31,
        K_D = 32,
        K_E = 18,

        K_UP    = 103,
        K_DOWN  = 108,
        K_LEFT  = 105,
        K_RIGHT = 106,

        K_ESC = 1,
        K_SHIFT = 42,
        K_LCTRL = 29,
        K_RCTRL = 97,

        K_1 = 2,
        K_2 = 3,
        K_3 = 4,
    }

    public enum types : short {
        EV_KEY = 1,
    }

    public enum values : int {
        KEY_UP = 0,
        KEY_DOWN = 1,
        KEY_HELD = 2,
    }
}
