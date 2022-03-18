using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;

namespace rpg_game.maze.Items
{
    class Iron_Armor : Item
    {
        public Iron_Armor()
        {
            name = "Iron Armor";
            addHP = 5;

            imageID = 2;

            tags[equipSlots.Torso] = true;
        }
    }
}
