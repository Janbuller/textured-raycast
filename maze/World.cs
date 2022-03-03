using System.Collections.Generic;
using textured_raycast.maze.math;

namespace textured_raycast.maze
{
    public enum states
    {
        Game,
        Inventory,
        Fighting,
        Paused,
        Settings,
        Stopping,
    }

    internal class World
    {
        public int currentMap = 1;
        private Dictionary<int, Map> maps = new Dictionary<int, Map>()
        {
            {1, new Map("maps/overworld.map")}
        };

        public int uiIndex = 1;
        public states state = states.Game;

        public string currentMessage = ""; // this is to show messages to the player or something
        public string interactMessage = ""; // This takes priority over current message

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
