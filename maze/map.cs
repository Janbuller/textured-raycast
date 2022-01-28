using System.Collections.Generic;
using System.Linq;

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
        public List<int> map = new List<int>();

        public Map(int sizeX, int sizeY) {
            width = sizeX;
            height = sizeY;

            // Initialize map to empty list of correct size.
            map = new int[width * height].ToList();
        }

        public Map(int sizeX, int sizeY, int[] map) {
            width = sizeX;
            height = sizeY;

            this.map = map.ToList();
        }

        // Gets specific cell.
        public int GetCell(int x, int y) {
            return map[width-x + y * width];
        }

        // Original SetCell function. Different X and Y from GetCell, since
        // GetCell was changed to flip horizontally.
        public void SetCell(int x, int y, int cell) {
            map[x + y * width] = cell;
        }

        // SetCell relative to GetCell.
        public void SetCellRel(int x, int y, int cell) {
            map[width-x + y * width] = cell;
        }
    }
}
