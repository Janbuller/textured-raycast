using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites.allSprites;
using System.Threading.Tasks;
using System.Diagnostics;

namespace textured_raycast.maze.lights
{
    class LightDistHelpers {
        public static TexColor MixLightDist(LightDist[] lights) {
            TexColor mixedCol = new TexColor(0, 0, 0);

            // I tried using a Parallel.For loop, but the overhead of starting
            // threads actually made it slower.
            for(int i = 0; i < lights.Count(); i++) {
                LightDist light = lights[i];
                TexColor curCol = light.col * (float)light.intensity;

                float distScalar = (float)(1 / (0.35 * light.dist + 0.44 * light.dist * light.dist));

                mixedCol += curCol * distScalar;
            }
            return mixedCol;
        }

        public static LightDist[] RoofLightArrayToDistArray(ILight[] lights, Vector2d toPos) {
            List<LightDist> lightDists = new List<LightDist>();
            // I tried using a Parallel.For loop, but the overhead of starting
            // threads actually made it slower.
            for(int i = 0; i < lights.Count(); i++) {
                double distTo = lights[i].GetLightPos().DistTo(toPos);
                distTo = distTo == 0 ? 0.00001 : distTo;
                if(distTo > 25)
                    continue;
                lightDists.Add(new LightDist(
                    distTo,
                    lights[i].GetLightColor(),
                    lights[i].GetLightIntensity()
                ));
            }

            return lightDists.ToArray();
        }
    }
}
