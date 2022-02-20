using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze;
using textured_raycast.maze.math;

namespace textured_raycast.maze
{
    internal class World
    {
        public int currentMap = 1;
        private Dictionary<int, Map> maps = new Dictionary<int, Map>()
        {
            {1, new Map("maps/overworld.map")},
            {2, new Map("maps/someHiddenPlace.map")},
        };

        public Vector2d plrPos;
        public Vector2d plrRot;

        public World()
        {
            plrPos = maps[currentMap].playerStartPos;
            plrRot = maps[currentMap].playerStartRot;
        }

        public Map getMapByID(int id)
        {
            return maps[id];
        }
    }
}
