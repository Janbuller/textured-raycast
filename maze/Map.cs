using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using System.Globalization;
using textured_raycast.maze.sprites.allSprites;

namespace textured_raycast.maze
{
    class Map
    {
        // Save width and height.
        int width, height;
        // Don't allow changing.
        public int Width { get => width; }
        public int Height { get => height; }
        // Map is śaved as a list of ints. Different numbers have different
        // functions/colors.
        public List<Wall> roof = new List<Wall>();
        public List<Wall> map = new List<Wall>();
        public List<Wall> floor = new List<Wall>();
        public List<Sprite> sprites = new List<Sprite>();

        public Dictionary<int, Vector2d> doorPositions = new Dictionary<int, Vector2d>();
        public List<int> lightPoitions = new List<int>();

        public int floorTexID = 1;
        public int ceilTexID = 1;

        public Vector2d playerStartPos;
        public Vector2d playerStartRot;

        public float lightMix = 0.7f;

	public string Path;

        private Dictionary<int, Type> spriteTypes = new Dictionary<int, Type>(){
            {0, typeof(DefaultSprite)},
            {1, typeof(Door)},
            {2, typeof(RoofLight)},
            {3, typeof(Enemy)},
            {4, typeof(TalkingSprite)},
            {5, typeof(Portal)},
            {6, typeof(Fireball)},
            {7, typeof(RotatingSprite)},
            {8, typeof(ChoiceTP)},
            {9, typeof(Shop)},
        };

        public bool useSkybox = true;

        public Map(string location)
        {
            string[] imageData = File.ReadAllLines(location);

            Path = location;

            int reqWidth;
            int reqHeight;
            double plrStartX;
            double plrStartY;
            double plrStartDX;
            double plrStartDY;

            if (!int.TryParse(imageData[0].Split(' ')[0], out reqWidth))
                return;
            if (!int.TryParse(imageData[0].Split(' ')[1], out reqHeight))
                return;
            if (!double.TryParse(imageData[1].Split(' ')[0], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartX))
                return;
            if (!double.TryParse(imageData[1].Split(' ')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartY))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[0], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartDX))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[1], NumberStyles.Any, CultureInfo.InvariantCulture, out plrStartDY))
                return;

            width = reqWidth;
            height = reqHeight;

            playerStartPos = new Vector2d(plrStartX, plrStartY);
            playerStartRot = new Vector2d(plrStartDX, plrStartDY);

            // Initialize map to empty list of correct size.
            roof = new Wall[width * height].ToList();
            map = new Wall[width * height].ToList();
            floor = new Wall[width * height].ToList();

            for (int i = 3; i < map.Count + 3; i++)
            {
                string[] paths = imageData[i].Split(' ');

                floor[i - 3] = new Wall(paths[0]);
                map[i - 3] = new Wall(paths[1]);
                roof[i - 3] = new Wall(paths[2]);
            }

            for (int i = map.Count + 3; i < imageData.Length; i++)
            {
                string[] thisInfo = imageData[i].Split(' ');
                int thisID = 0;

                if (thisInfo.Length == 3)
                {
                    thisID = 0;
                }
                else
                {
                    thisID = int.Parse(thisInfo[3]);
                }



                if (thisInfo.Length == 3)
                {
                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), 0, "") as Sprite);
                }
                else if (thisInfo.Length == 4)
                {
                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), int.Parse(thisInfo[3]), "") as Sprite);
                }
                else if (thisInfo.Length != 1)
                {
                    string thisString = "";
                    for (int i2 = 4; i2 < thisInfo.Length; i2++)
                    {
                        thisString += thisInfo[i2] + " ";
                    }
                    if (thisString.Length > 0)
                        thisString = thisString.Substring(0, thisString.Length - 1);


                    sprites.Add(Activator.CreateInstance(spriteTypes[thisID], double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), thisInfo[2].Split('-'), int.Parse(thisInfo[3]), thisString) as Sprite);

                    if (sprites[sprites.Count - 1].effectID == 1)
                        doorPositions.Add(sprites[sprites.Count - 1].extraEffects[2], sprites[sprites.Count - 1].getPos());

                    if (sprites[sprites.Count - 1].effectID == 2)
                        lightPoitions.Add(sprites.Count - 1);
                }
            }
        }

        public ILight[] GetLights()
        {
            List<int> lightIdx = lightPoitions;
            ILight[] lights = new ILight[lightIdx.Count];

            for (int i = 0; i < lightIdx.Count; i++)
            {
                lights[i] = (ILight)sprites[lightIdx[i]];
            }

            return lights;
        }

        // Return if specific cell is a wall / should be drawn.
        public bool IsWall(int x, int y)
        {
            try
            {
                return map[x + y * width].isWall;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public string GetFloor(int x, int y)
        {
            if (x > width - 1 || x < 0 || y > height - 1 || y < 0)
                return "";
            else
                return floor[x + y * width].textPath;
        }
        public string GetRoof(int x, int y)
        {
            if (x > width - 1 || x < 0 || y > height - 1 || y < 0)
                return "";
            else
                return roof[x + y * width].textPath;
        }

        public void openDoor(int myID, int doorID)
        {
            resetSprites();
            World.currentMap = myID;
            World.plrPos = new Vector2d(doorPositions[doorID].x, doorPositions[doorID].y);
        }

        public void fullReset()
        {
            resetSprites();
            World.resetPlrPos();
        }

        public void resetSprites()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.resetSprite();
            }
        }

        // Gets specific cell.
        public Wall GetWall(int x, int y)
        {
            return map[x + y * width];
        }

        // Original SetCell function. Different X and Y from GetCell, since
        // GetCell was changed to flip horizontally.
        public void SetCell(int x, int y, string texture)
        {
            map[x + y * width] = new Wall(texture);
        }

        // SetCell relative to GetCell.
        public void SetCellRel(int x, int y, string texture)
        {
            map[width - x + y * width] = new Wall(texture);
        }
    }
}
