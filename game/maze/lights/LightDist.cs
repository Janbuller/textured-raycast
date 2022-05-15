using textured_raycast.maze.texture;

namespace textured_raycast.maze.lights
{
    struct LightDist {
        public double dist;
        public TexColor col;
        public double intensity;

        public double linear;
        public double quadratic;

        public LightDist(double dist, TexColor col, double intensity = 100, double linear = 22, double quadratic = 20) {
            this.dist = dist;
            this.col = col;
            this.intensity = intensity;

        this.linear = linear;
        this.quadratic = quadratic;
        }
    }
}
