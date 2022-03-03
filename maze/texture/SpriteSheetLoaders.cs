using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace textured_raycast.maze.texture
{
    /// <summary>
    /// "Collection" of sprite sheet loaders for different file formats. (currently
    /// only supporting one...)
    /// </summary>
    class SpriteSheetLoaders {
        /// <summary>
        /// Creates a list of textures from a PPM image
        /// </summary>
        public static List<Texture> loadHorisontalSpriteSheetFromPlainPPM(string location, int width) {
            Texture baseTexture = TextureLoaders.loadFromPlainPPM(location);

            List<Texture> textures = new List<Texture>();

            int curWidth = 0;
            while (curWidth < baseTexture.width)
            {
                List<TexColor> texColors = new List<TexColor>();

                for (int y = 0; y < baseTexture.height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        texColors.Add(baseTexture.getPixel(x + (textures.Count)*width, y));
                    }
                }

                Texture texture = new Texture(texColors, width, baseTexture.height);

                textures.Add(texture);

                curWidth += width;
            }

            return textures;
        }
    }
}
