using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Drawing;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.texture
{
    class TextureHelper {
        public static Texture HalfScale(Texture Downscaled) {
            List<TexColor> imageData = new List<TexColor>();
            for(int y = 0; y < Downscaled.height; y+=2) {
                for(int x = 0; x < Downscaled.width; x+=2) {
                    TexColor pixel1 = Downscaled.getPixel(x, y);
                    TexColor pixel2 = Downscaled.getPixel(x+1, y);
                    TexColor pixel3 = Downscaled.getPixel(x, y+1);
                    TexColor pixel4 = Downscaled.getPixel(x+1, y+1);

                    int newR = pixel1.r + pixel2.r + pixel3.r + pixel4.r;
                    newR /= 4;
                    int newG = pixel1.g + pixel2.g + pixel3.g + pixel4.g;
                    newG /= 4;
                    int newB = pixel1.b + pixel2.b + pixel3.b + pixel4.b;
                    newB /= 4;
                    TexColor newPixel = new TexColor(newR, newG, newB);
                    imageData.Add(newPixel);
                }
            }

            return new Texture(imageData, Downscaled.width/2, Downscaled.height/2);
        }
    }
}
