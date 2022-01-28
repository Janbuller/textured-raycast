using System;
using System.Threading;
using System.Collections.Generic;
using textured_raycast.maze;
using textured_raycast.maze.texture;

namespace textured_raycast
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] mapArr = {
                1, 1,   1,   3, 3, 3, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1,   0,   0, 0, 3, 0, 4, 0, 1, 1, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1,
                1, 1,   0,   2, 0, 3, 0, 4, 0, 0, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1,
                1, 102, 100, 2, 0, 3, 0, 0, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 101,
                1, 1,   0,   0, 0, 3, 0, 4, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 0, 1, 0, 0, 1, 1, 1, 0, 0, 1, 1,
                1, 1,   0,   0, 0, 0, 0, 4, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 1,
                1, 1,   5,   5, 5, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
            };
            Map map = new Map(29, 7, mapArr);
            Maze.StartMaze(map);

            // Texture test = TextureLoaders.loadFromPlainPPM("img/test.ppm");
            // test.draw();

        }
    }
}
