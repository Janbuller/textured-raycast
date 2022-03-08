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
                // Draw the line, using NuGet package "Pastel" to color, using
                // ansi escape sequences.
                if(drawToCol != texToDraw.getPixel(x, i))
                    continue;
                const float mixBy = 0.3f;
                texToDraw.setPixel(x, i, (color * darken * mixBy + TexColor.unitMult(color, light) * (1 - mixBy)));
            }
        }
    }
}
