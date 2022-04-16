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

        public enum GetPixelMode
        {
	    BLACK,
	    REPEAT,
        }
        public TexColor getPixel(int x, int y, GetPixelMode mode) {
            TexColor retPixel = null;

            if (x > 0 && x < width &&
                y > 0 && y < height)
            {
                retPixel = pixels[x + y * width];
            } else
            {
                if (mode == GetPixelMode.BLACK)
                {
		    retPixel = new TexColor(0, 0, 0);
                } else if(mode == GetPixelMode.REPEAT)
		{
                    int newX = 0, newY = 0;
		    while(x < 0)
			newX += width;
		    while(x > width)
                        newX -= width;

		    while(y < 0)
			newY += width;
		    while(y > width)
                        newY -= width;

                    retPixel = pixels[newX + newY * width];
                }
            }

            return retPixel;
        }

        public void Clear() {
            this.pixels = new TexColor[width*height].ToList();
        }

        public Texture getGreyscale()
        {
            TexColor[] pixels = new TexColor[this.pixels.Count];
            for (int i = 0; i < this.pixels.Count; i++)
            {
		int value = TexColor.getBrightness(this.pixels[i]);
                pixels[i] = new TexColor(value, value, value);
            }
            return new Texture(pixels.ToList(), width, height);
        }
        public void setPixel(int x, int y, TexColor pixel) {
            if(x < 0 || x > width || y < 0 || y > height) {
                return;
            }
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
