using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;
using System.Globalization;

namespace textured_raycast.maze
{
    class Map {
        // Save width and height.
        int width, height;
        // Don't allow changing.
        public int Width{get => width;}
        public int Height{get => height;}
        // Map is śaved as a list of ints. Different numbers have different
        // functions/colors.
        public List<Wall> map = new List<Wall>();
        public List<Sprite> sprites = new List<Sprite>();

        public Dictionary<int, Vector2d> doorPositions = new Dictionary<int, Vector2d>();

        public int floorTexID;
        public int ceilTexID;

        public Vector2d playerStartPos;
        public Vector2d playerStartRot;

        public bool useSkybox = true;

        public Map(string location)
        {
            string[] imageData = File.ReadAllLines(location);

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
            if (!int.TryParse(imageData[0].Split(' ')[2], out floorTexID))
                return;
            if (imageData[0].Split(' ').Length == 4)
            {
                useSkybox = false;
                if (!int.TryParse(imageData[0].Split(' ')[3], out ceilTexID))
                    return;
            }
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
            map = new Wall[width * height].ToList();

            for (int i = 3; i < map.Count + 3; i++)
            {
                map[i - 3] = new Wall(int.Parse(imageData[i]));
            }

            for (int i = map.Count + 3; i < imageData.Length; i++)
            {
                Console.WriteLine(imageData[i]);
                string[] thisInfo = imageData[i].Split(' ');
                if (thisInfo.Length == 3)
                {
                    sprites.Add(new Sprite(double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), int.Parse(thisInfo[2])));
                }
                else if (thisInfo.Length == 4)
                {
                    sprites.Add(new Sprite(double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), int.Parse(thisInfo[2]), effectID: int.Parse(thisInfo[3])));
                }
                else if (thisInfo.Length != 1)
                {
                    string thisString = "";
                    for (int i2 = 4; i2 < thisInfo.Length; i2++)
                    {
                        thisString+=thisInfo[i2]+" ";
                    }
                    if (thisString.Length > 0)
                        thisString = thisString.Substring(0, thisString.Length - 1);


                    sprites.Add(new Sprite(double.Parse(thisInfo[0], CultureInfo.InvariantCulture), double.Parse(thisInfo[1], CultureInfo.InvariantCulture), int.Parse(thisInfo[2]), effectID: int.Parse(thisInfo[3]), whatsLeft: thisString));

                    if (sprites[sprites.Count - 1].effectID == 1)
                        doorPositions.Add(sprites[sprites.Count - 1].extraEffects[2], sprites[sprites.Count - 1].getPos());
                }
            }
        }

        // Return if specific cell is a wall / should be drawn.
        public bool IsWal(int x, int y)
        {
            return map[x + y * width].isWal;
        }

        public void openDoor(ref World world, int myID, int doorID)
        {
            resetSprites();
            world.currentMap = myID;
            world.plrPos = new Vector2d(doorPositions[doorID].x, doorPositions[doorID].y);
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
        public void SetCell(int x, int y, int wallIDIn) {
            map[x + y * width] = new Wall(wallIDIn);
        }

        // SetCell relative to GetCell.
        public void SetCellRel(int x, int y, int wallIDIn) {
            map[width-x + y * width] = new Wall(wallIDIn);
        }
    }
}
