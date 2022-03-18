using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;

namespace rpg_game.maze.Items
{
    class Ruby_Ring : Item
    {
        public Ruby_Ring()
        {
            name = "Ruby Ring";
            addMAG = 36;

            imageID = 1;

            tags[equipSlots.Accessory1] = true;
            tags[equipSlots.Accessory2] = true;
        }
    }
}
