using System;
using System.Linq;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;
using textured_raycast.maze.sprites.allSprites;

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

                float distScalar = (float)(1 / (light.linear * light.dist + light.quadratic * light.dist * light.dist));

                mixedCol += curCol * distScalar;
            }

            return mixedCol;
        }

        public static LightDist[] RoofLightArrayToDistArray(ILight[] lights, Vector2d toPos) {
            bool PlayerLight = World.player.HoldsLight;

            LightDist[] lightDists = new LightDist[lights.Count() + Convert.ToInt32(PlayerLight)];

            // I tried using a Parallel.For loop, but the overhead of starting
            // threads actually made it slower.
            for(int i = 0; i < lights.Count(); i++) {
                double distTo = lights[i].GetLightPos().DistTo(toPos);
                distTo = distTo == 0 ? 0.00001 : distTo;
                lightDists[i] = new LightDist(
                    distTo,
                    lights[i].GetLightColor(),
                    lights[i].GetLightIntensity(),
		    lights[i].GetAttenuationLinear(),
		    lights[i].GetAttenuationQuadratic()
                );
            }

	    if(PlayerLight) {
                lightDists[lights.Count()] = new LightDist(
		    World.plrPos.DistTo(toPos),
		    new TexColor(255, 120, 0),
		    500,
		    35,
		    44
		);
            }

            return lightDists;
        }
    }
}
