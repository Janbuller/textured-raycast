using System;
using System.Collections.Generic;
using System.Linq;
using Pastel;

namespace textured_raycast.maze.texture
{
    class Texture {
        public int width, height;
        List<TexColor> pixels;

        public Texture(List<TexColor> pixels, int width, int height) {
            this.width = width;
            this.height = height;
            this.pixels = pixels;
        }

        public Texture(int width, int height) {
            this.width = width;
            this.height = height;
            this.pixels = new TexColor[width*height].ToList();
        }

        public Texture(Texture tex) {
            this.width = tex.width;
            this.height = tex.height;
            this.pixels = new List<TexColor>(tex.pixels);
        }

        public TexColor getPixel(int x, int y) {
            return pixels[x + y * width];
        }

        public void setPixel(int x, int y, TexColor pixel) {
            pixels[x + y * width] = pixel;
        }

        public void draw() {
            for(int y = 0; y < height; y++) {
                for(int x = 0; x < width; x++) {
                    TexColor pixel = getPixel(x, y);
                    Console.Write("██".Pastel(pixel.getSysColor()));
                }
                Console.WriteLine();
            }
        }
    }
}
