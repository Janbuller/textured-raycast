using System.Collections.Generic;

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

                    int newR = pixel1.R + pixel2.R + pixel3.R + pixel4.R;
                    newR /= 4;
                    int newG = pixel1.G + pixel2.G + pixel3.G + pixel4.G;
                    newG /= 4;
                    int newB = pixel1.B + pixel2.B + pixel3.B + pixel4.B;
                    newB /= 4;
                    TexColor newPixel = new TexColor(newR, newG, newB);
                    imageData.Add(newPixel);
                }
            }

            return new Texture(imageData, Downscaled.width/2, Downscaled.height/2);
        }

        public static Texture DoubleScale(Texture Upscaled) {
            List<TexColor> imageData = new List<TexColor>();
            for(int y = 0; y < Upscaled.height*2; y++) {
                for(int x = 0; x < Upscaled.width; x++) {
                    TexColor pixel = Upscaled.getPixel(x, (int)(y/2));

                    imageData.Add(pixel);
                    imageData.Add(pixel);
                }
            }

            return new Texture(imageData, Upscaled.width*2, Upscaled.height*2);
        }

        public static Texture TripleScale(Texture Upscaled) {
            List<TexColor> imageData = new List<TexColor>();
            for(int y = 0; y < Upscaled.height*3; y++) {
                for(int x = 0; x < Upscaled.width; x++) {
                    TexColor pixel = Upscaled.getPixel(x, (int)(y/3));

                    imageData.Add(pixel);
                    imageData.Add(pixel);
                    imageData.Add(pixel);
                }
            }

            return new Texture(imageData, Upscaled.width*3, Upscaled.height*3);
        }

        public static void DrawVerLine(ref Texture texToDraw, int x, int height, Texture tex, int texX, float darken, TexColor drawToCol = null) {
            int startY = texToDraw.height/2 - height/2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - texToDraw.height / 2 + height /2) * sectionHeight;
            if(startY < 0) {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > texToDraw.height ? texToDraw.height : endY;
            for(int i = startY; i < endY; i++) {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                if(drawToCol != texToDraw.getPixel(x, i))
                    continue;
                texToDraw.setPixel(x, i, (color * darken));
            }
        }

        public static void DrawVerLine(ref Texture texToDraw, int x, int height, Texture tex, int texX, float darken, TexColor light, TexColor drawToCol = null) {
            int startY = texToDraw.height/2 - height/2;
            int endY = height + startY;

            float sectionHeight = (float)tex.height / height;
            float texPos = (startY - texToDraw.height / 2 + height /2) * sectionHeight;
            if(startY < 0) {
                texPos += sectionHeight * (startY * -1);
            }
            startY = startY < 0 ? 0 : startY;
            endY = endY > texToDraw.height ? texToDraw.height : endY;
            for(int i = startY; i < endY; i++) {
                int texY = (int)texPos;
                texPos += sectionHeight;
                TexColor color = tex.getPixel(texX, texY);
                if(color is null)
                    continue;
                if(drawToCol != texToDraw.getPixel(x, i))
                    continue;
                const float mixBy = 0.3f;
                texToDraw.setPixel(x, i, (color * darken * mixBy + TexColor.unitMult(color, light) * (1 - mixBy)));
            }
        }
    }
}
