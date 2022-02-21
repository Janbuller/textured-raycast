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

        public Vector2d multByVec(Vector2d vec) {
            return new Vector2d(
                getE(0, 0) * vec.x + getE(1,0) * vec.y,
                getE(0, 1) * vec.x + getE(1,1) * vec.y
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
    }
}
