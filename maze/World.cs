using System.Collections.Generic;
using textured_raycast.maze.math;
using System;
using textured_raycast.maze.sprites;
using textured_raycast.maze.resources;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.Utils;

namespace textured_raycast.maze
{
    public enum States
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
        public static ConsoleEngine ce;
	public static Vector2i WindowSize = new Vector2i(120, 80);

        public static int currentMap = 1;
        private static Dictionary<int, string> maps;

        public static States state = States.Game;

        public static string currentMessage = ""; // this is to show messages to the player or something
        public static string interactMessage = ""; // This takes priority over current message

        public static Fight.Fight fight;

        public static Player player = new Player();

        public static Map curMap;

        public static float plrBobTime = 0;
        public static float plrBob = 0;

        public static Vector2d plrPos;
        public static Vector2d plrRot;
        public static Vector2d plrPlane;
        public static float staminaLVL = 1;
        public static float dayTime = 0;

        public static double dt;
        public static double timePassed;
        private static DeltaVariable dtCalc = new DeltaVariable(new double[] {GetRealTime()});

        public static Dictionary<int, string> textures = new Dictionary<int, string>() {
            {1,   "img/ownwalls/bricks.ppm"},
            {2,   "img/ownwalls/redbricks.ppm"},
            {3,   "img/wolfenstein/bluestone.ppm"},
            {4,   "img/test5.ppm"},
            {5,   "img/wolfenstein/redstone.ppm"},
            {6,   "img/ownwalls/stone.ppm"},
            {99,  "img/skybox.ppm"},
            {101, "img/wolfenstein/end.ppm"}, // Also used as collision box for winning.
            {102, "img/wolfenstein/exit.ppm"}, // Also used for leaving the maze
            {103, "img/INV.ppm"},
            {104, "img/SkillTree.ppm"},
        };

        public static void startFight(Sprite sprite)
        {
            state = States.Fighting;
            fight = new Fight.Fight(sprite as Enemy);
        }

        public static void resetPlrPos()
        {
            curMap = ResourceManager.getMap(maps[currentMap]);
            plrPos = new Vector2d(curMap.playerStartPos);
            plrRot = new Vector2d(curMap.playerStartRot);
            plrPlane = new Vector2d(plrRot.Y, -plrRot.X) * 0.66;
        }

        public static void setupMapsInWorld()
        {
            maps = new Dictionary<int, string>()
            {
                {-1, "maps/fightMap.map"},
                {1, "maps/StartingCave.map"},
                {2, "maps/TheHolyLands.map"},
            };

            resetPlrPos();
        }
        public static void openMapAtStartPos(Map map)
        {
            curMap = map;
            plrPos = new Vector2d(curMap.playerStartPos);
            plrRot = new Vector2d(curMap.playerStartRot);
            plrPlane = new Vector2d(plrRot.Y, -plrRot.X) * 0.66;
        }

        public static void reloadCurMap()
        {
            curMap = new Map(curMap.Path);
        }

        public static Map getMapByID(int id)
        {
            return ResourceManager.getMap(maps[id]);
        }
        
        public static Map getCurMap()
        {
            return curMap;
        }

	public static double GetRealTime() {
            return (double)DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
        }

	public static void UpdateDeltaTime() {
            dt = dtCalc.GetDelta(new double[] { GetRealTime() })[0];
        }
    }
}
