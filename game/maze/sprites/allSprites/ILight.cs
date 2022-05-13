using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    // an interface for all sprites that need to make light
    interface ILight
    {
        public TexColor GetLightColor();
        public float GetLightIntensity();
        public Vector2d GetLightPos();

        public float GetAttenuationLinear();
        public float GetAttenuationQuadratic();
    }
}
