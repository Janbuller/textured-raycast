using System;
using System.Drawing;

namespace textured_raycast.maze.math
{
    // Vector 2 double.
    class Matrix2x2d {
        // Holds x and y of vector.
        public double[] elements = new double[4];

        public Matrix2x2d(double[] elements) {
            this.elements = elements;
        }

        public double getE(int x, int y) {
            return elements[y * 2 + x];
        }

        public void setE(int x, int y, double value) {
            elements[y * 2 + x] = value;
        }

        public Vector2<double> multByVec(Vector2<double> vec) {
            return new Vector2<double>(
                getE(0, 0) * vec.x + getE(1,0) * vec.y,
                getE(0, 1) * vec.x + getE(1,1) * vec.y
            );
        }
    }
}
