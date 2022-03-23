using System.Collections.Generic;
using System.IO;
using textured_raycast.maze.texture;

namespace textured_raycast.maze.resources
{
    class ResourceManager
    {
        private static Dictionary<string, Texture> cachedTextures = new Dictionary<string, Texture>(){};
        private static Dictionary<string, Map> cachedMaps = new Dictionary<string, Map>(){};

        public static Texture getTexture(string path)
        {
            Texture tex;
            bool exists = cachedTextures.TryGetValue(path, out tex);
            if (!exists)
            {
                try
                {
                    tex = TextureLoaders.loadFromPlainPPM(path);
                } catch (FileNotFoundException)
                {
                    return null;
                }
                cachedTextures.Add(path, tex);
            }

            return tex;
        }

	/// Adds a texture to the texture cache. Make sure, /NOT/ to
	/// add multiple textures to the same path or to the path of
	/// and existing or futurely loaded texture.
        public static void cacheTexture(string path, Texture tex)
        {
	    cachedTextures.Add(path, tex);
        }

        public static Map getMap(string path)
        {
            Map map;
            bool exists = cachedMaps.TryGetValue(path, out map);
            if (!exists)
            {
                map = new Map(path);
                cachedMaps.Add(path, map);
            }

            return map;
        }
    }
}
