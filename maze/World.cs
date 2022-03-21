using System.Collections.Generic;
using textured_raycast.maze.math;
using System;
using rpg_game.maze.Fight;
using textured_raycast.maze.sprites;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;
using rpg_game.maze;

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

    class World
    {
        public static int currentMap = 1;
        private static Dictionary<int, string> maps;

        public static states state = states.Game;

        public static string currentMessage = ""; // this is to show messages to the player or something
        public static string interactMessage = ""; // This takes priority over current message

        public static Fight fight;

        public static Player player = new Player();

        public static Vector2d plrPos;
        public static Vector2d plrRot;
        public static Vector2d plrPlane;
        public static float staminaLVL = 1;
        public static float dayTime = 0;

        public static float dt;
        public static long lastFrameTime = DateTime.Now.Ticks;

        public static Dictionary<int, string> textures = new Dictionary<int, string>() {
            {1,   "img/wolfenstein/greystone.ppm"},
            {2,   "img/wolfenstein/redbrick.ppm"},
            {3,   "img/wolfenstein/bluestone.ppm"},
            {4,   "img/test5.ppm"},
            {5,   "img/wolfenstein/redstone.ppm"},
            {6,   "img/wolfenstein/colorstone.ppm"},
            {99,  "img/skybox.ppm"},
            {101, "img/wolfenstein/end.ppm"}, // Also used as collision box for winning.
            {102, "img/wolfenstein/exit.ppm"}, // Also used for leaving the maze
            {103, "img/INV.ppm"},
            {104, "img/SkillTree.ppm"},
        };

        public static void startFight(Sprite sprite)
        {
            state = states.Fighting;
            fight = new Fight(sprite as Enemy);
        }

        public static void resetPlrPos()
        {
            Map curMap = ResourceManager.getMap(maps[currentMap]);
            plrPos = new Vector2d(curMap.playerStartPos);
            plrRot = new Vector2d(curMap.playerStartRot);
            plrPlane = new Vector2d(plrRot.y, -plrRot.x) * 0.66;
        }

        public static void setupMapsInWorld()
        {
            maps = new Dictionary<int, string>()
            {
                {-1, "maps/fightMap.map"},
                {1, "maps/TheHolyLands.map"},
            };

            resetPlrPos();
        }

        public static Map getMapByID(int id)
        {
            return ResourceManager.getMap(maps[id]);
        }
    }
}
