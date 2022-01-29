using System;
using System.Collections.Generic;
using System.Text;

namespace textured_raycast.maze
{
    internal class Wall
    {
        public bool isWal = true;
        public int wallID;
        public int thisTexID;

        public Wall(int wallIDIn)
        {
            // TODO: do something based on the wall id

            wallID = wallIDIn;
            thisTexID = 1;

            if (wallID == 0)
                isWal = false;
        }
    }
}
