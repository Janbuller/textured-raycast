using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Drawing;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using Pastel;

namespace textured_raycast.maze.input
{
    interface ILowLevelInput {
        KeyState GetKey(Keys key);
        void Init();
    }
}
