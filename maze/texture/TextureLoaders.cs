using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Drawing;
using textured_raycast.maze.math;

namespace textured_raycast.maze.texture
{
    class TextureLoaders {
        public static Texture loadFromPlainPPM(string location) {
            string[] imageData = File.ReadAllLines(location);

            // Check magic number, to see if correct format
            if (imageData[0] != "P3")
                return null;

            // Remove all comments
            imageData = imageData.Where(line => line.Substring(0,1) != "#").ToArray();

            // Get width and height from image
            int width, height;
            string[] sizes = imageData[1].Split(" ");
            width  = Convert.ToInt32(sizes[0]);
            height = Convert.ToInt32(sizes[1]);

            int maxColorVal;
            maxColorVal = Convert.ToInt32(imageData[2]);

            TexColor[] pixels = new TexColor[width * height];

            for(int i = 3; i < imageData.Length; i += 3) {
                int[] colors = new int[3];
                for(int j = 0; j < 3; j++) {
                    colors[j] = Convert.ToInt32(imageData[i+j]);

                }
                TexColor color = new TexColor(colors[0], colors[1], colors[2], maxColorVal);
                pixels[(i-3)/3] = color;
            }

            return new Texture(pixels.ToList(), width, height);
        }
    }
}
