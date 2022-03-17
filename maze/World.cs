﻿using System.Collections.Generic;
using textured_raycast.maze.math;
using System;
using rpg_game.maze.Fight;
using textured_raycast.maze.sprites;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;

namespace textured_raycast.maze
{
    public enum states
    {
        Game,
        Inventory,
        Skills,
        Fighting,
        Paused,
        Settings,
        Stopping,
    }

    internal class World
    {
        public int currentMap = 1;
        private Dictionary<int, string> maps;

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
            maps = new Dictionary<int, string>()
            {
                {-1, "maps/fightMap.map"},
                // {1, "maps/overworld.map"},
                {1, "maps/lightTest.map"},
                // {2, "maps/DarkDungeon.map"},
                {3, "maps/hiddenRoom.map"},
            };

            Map curMap = ResourceManager.getMap(maps[currentMap], this);
            plrPos = curMap.playerStartPos;
            plrRot = curMap.playerStartRot;
        }

        public Map getMapByID(int id)
        {
            return ResourceManager.getMap(maps[id], this);
        }
    }
}
