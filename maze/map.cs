using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using textured_raycast;
using textured_raycast.maze.math;

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

        public Vector2d playerStartPos;
        public Vector2d playerStartRot;

        public Map(string location)
        {
            string[] imageData = File.ReadAllLines(location);

            int reqWidth;
            int reqHeight;
            float plrStartX;
            float plrStartY;
            float plrStartDX;
            float plrStartDY;

            if (!int.TryParse(imageData[0], out reqWidth))
                return;
            if (!int.TryParse(imageData[1], out reqHeight))
                return;
            if (!float.TryParse(imageData[2], out plrStartX))
                return;
            if (!float.TryParse(imageData[3], out plrStartY))
                return;
            if (!float.TryParse(imageData[4], out plrStartDX))
                return;
            if (!float.TryParse(imageData[5], out plrStartDY))
                return;

            width = reqWidth;
            height = reqHeight;

            playerStartPos = new Vector2d(plrStartX, plrStartY);
            playerStartRot = new Vector2d(plrStartDX, plrStartDY);

            // Initialize map to empty list of correct size.
            map = new Wall[width * height].ToList();

            for (int i = 6; i < imageData.Length; i++)
            {
                map[i - 6] = new Wall(int.Parse(imageData[i]));
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
