using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.lights;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.GUI;
using System.Threading.Tasks;
using rpg_game.maze;
using rpg_game.maze.ButtonList.Buttons.INV;
using rpg_game.maze.ButtonList.Buttons.Skills;

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
                tex = TextureLoaders.loadFromPlainPPM(path);
                cachedTextures.Add(path, tex);
            }

            return tex;
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
