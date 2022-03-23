using System;
using System.Collections.Generic;
using System.Linq;
using textured_raycast.maze.math;
using textured_raycast.maze.texture;
using textured_raycast.maze.sprites;
using textured_raycast.maze.sprites.allSprites;
using textured_raycast.maze.input;
using textured_raycast.maze.GUI;

namespace textured_raycast.maze.lights
{
    struct LightDist {
        public double dist;
        public TexColor col;
        public double intensity;

        public double linear;
        public double quadratic;

        public LightDist(double dist, TexColor col, double intensity, double linear, double quadratic) {
            this.dist = dist;
            this.col = col;
            this.intensity = intensity;

	    this.linear = linear;
	    this.quadratic = quadratic;
        }
    }
}
