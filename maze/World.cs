using System.Collections.Generic;
using textured_raycast.maze.math;
using System;
using rpg_game.maze.Fight;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;

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
        private Dictionary<int, Map> maps;

        public states state = states.Game;

        public string currentMessage = ""; // this is to show messages to the player or something
        public string interactMessage = ""; // This takes priority over current message

        public Fight fight;

        public Vector2d plrPos;
        public Vector2d plrRot;
        public float staminaLVL = 1;
        public float dayTime = 0;

        public float dt;
        public long lastFrameTime = DateTime.Now.Ticks;

        public void startFight(Sprite sprite)
        {
            state = states.Fighting;
            fight = new Fight(sprite as Enemy);
        }

        public World()
        {
            maps = new Dictionary<int, Map>()
            {
                {-1, new Map("maps/fightMap.map", this)},
                // {1, new Map("maps/overworld.map", this)},
                {1, new Map("maps/lightTest.map", this)},
                // {2, new Map("maps/DarkDungeon.map", this)},
                {3, new Map("maps/hiddenRoom.map", this)},
            };

            plrPos = maps[currentMap].playerStartPos;
            plrRot = maps[currentMap].playerStartRot;
        }

        public Map getMapByID(int id)
        {
            return maps[id];
        }
    }
}
