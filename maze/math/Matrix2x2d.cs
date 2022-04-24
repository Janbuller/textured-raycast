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

        private static Vector2d multByVec(Vector2d vec, Matrix2x2d mat) {
            return new Vector2d(
                mat.getE(0, 0) * vec.X + mat.getE(1,0) * vec.Y,
                mat.getE(0, 1) * vec.X + mat.getE(1,1) * vec.Y
            );
        }


        public Matrix2x2d getInverse() {
            double determinant = 1/(getE(0,0) * getE(1,1) - getE(1,0) * getE(0,1));

            Matrix2x2d invMat = new Matrix2x2d(new double[] { getE(1,1), -getE(1,0),
                                                             -getE(0, 1), getE(0,0)});
            return invMat * determinant;
        }

        private static Matrix2x2d multByScalar(double scalar, Matrix2x2d mat) {
            double[] newElements = new double[4];
            for(int i = 0; i < mat.elements.Length; i++) {
                newElements[i] = mat.elements[i] * scalar;
            }
            return new Matrix2x2d(newElements);
        }

        public static Matrix2x2d operator *(double scalar, Matrix2x2d mat) {
            return multByScalar(scalar, mat);

        }
        public static Matrix2x2d operator *(Matrix2x2d mat, double scalar) {
            return multByScalar(scalar, mat);
        }

        public static Vector2d operator *(Vector2d vec, Matrix2x2d mat) {
            return multByVec(vec, mat);

        }
        public static Vector2d operator *(Matrix2x2d mat, Vector2d vec) {
            return multByVec(vec, mat);
        }
    }
}
