using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using textured_raycast;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites;

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

        public Vector2d playerStartPos;
        public Vector2d playerStartRot;

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
            if (!double.TryParse(imageData[1].Split(' ')[0], out plrStartX))
                return;
            if (!double.TryParse(imageData[1].Split(' ')[1], out plrStartY))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[0], out plrStartDX))
                return;
            if (!double.TryParse(imageData[2].Split(' ')[1], out plrStartDY))
                return;

            width = reqWidth;
            height = reqHeight;

            playerStartPos = new Vector2d(plrStartX, plrStartY);
            playerStartRot = new Vector2d(plrStartDX, plrStartDY);

            // Initialize map to empty list of correct size.
            map = new Wall[width * height].ToList();

            for (int i = 3; i < map.Count + 2; i++)
            {
                map[i - 3] = new Wall(int.Parse(imageData[i]));
            }

            for (int i = map.Count + 2; i < imageData.Length; i++)
            {
                string[] thisInfo = imageData[i].Split(' ');
                sprites.Add(new Sprite(double.Parse(thisInfo[0]), double.Parse(thisInfo[1]), int.Parse(thisInfo[2])));
            }
        }

        // Return if specific cell is a wall / should be drawn.
        public bool IsWal(int x, int y)
        {
            return map[x + y * width].isWal;
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
