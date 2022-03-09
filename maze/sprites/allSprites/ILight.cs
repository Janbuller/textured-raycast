using System;
using System.Collections.Generic;
using System.Text;
using textured_raycast.maze.sprites;
using textured_raycast.maze.texture;
using textured_raycast.maze.math;

namespace textured_raycast.maze.sprites.allSprites
{
    interface ILight
    {
        public TexColor GetLightColor();
        public float GetLightIntensity();
        public Vector2d GetLightPos();
    }
}
