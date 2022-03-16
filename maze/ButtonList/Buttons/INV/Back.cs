﻿using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;

namespace rpg_game.maze.ButtonList.Buttons.INV
{
    class Back : Button
    {
        public Back(int x, int y, int w, int h, int[] list) : base(x, y, w, h, list) {}

        public override void onActivate(World world)
        {
            world.state = states.Paused;
        }
    }
}