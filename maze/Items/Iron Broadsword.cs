using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;

namespace rpg_game.maze.Items
{
    class Iron_Broadsword : Item
    {
        public Iron_Broadsword()
        {
            name = "Iron Broadsword";
            addDAM = 3;

            tags[equipSlots.MainHand] = true;
            tags[equipSlots.OffHand] = true;
        }
    }
}
