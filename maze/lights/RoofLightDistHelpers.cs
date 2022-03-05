using System;
using System.Linq;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites.allSprites;
using System.Threading.Tasks;
using System.Diagnostics;

namespace textured_raycast.maze.lights
{
    class RoofLightDistHelpers {
        public static TexColor MixLightDist(RoofLightDist[] lights) {
            TexColor mixedCol = new TexColor(0, 0, 0);

            // I tried using a Parallel.For loop, but the overhead of starting
            // threads actually made it slower.
            for(int i = 0; i < lights.Count(); i++) {
                RoofLightDist light = lights[i];
                TexColor curCol = light.col * (float)light.intensity;

                float distScalar = (float)(1 / (0.7 * light.dist + 1.8 * light.dist * light.dist));

                mixedCol += curCol * distScalar;
            }
            return mixedCol;
        }

        public static RoofLightDist[] RoofLightArrayToDistArray(RoofLight[] lights, Vector2d toPos) {
            RoofLightDist[] lightDists = new RoofLightDist[lights.Count()];
            // I tried using a Parallel.For loop, but the overhead of starting
            // threads actually made it slower.
            for(int i = 0; i < lights.Count(); i++) {
                double distTo = lights[i].pos.DistTo(toPos);
                distTo = distTo == 0 ? 0.00001 : distTo;
                lightDists[i] = new RoofLightDist(
                    distTo,
                    lights[i].thisColor,
                    lights[i].intesity
                );
            }

            return lightDists;
        }
    }
}
