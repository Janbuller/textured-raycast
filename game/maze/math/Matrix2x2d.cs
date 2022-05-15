using System;

namespace textured_raycast.maze.math
{
    // Matrix 2x2 double.
    public class Matrix2x2d {
	// Holds the elements
        public double[] elements = new double[4];

        public Matrix2x2d(double[] elements) {
            this.elements = elements;
        }

	// Sets an element, based on 2D-coords.
        public void setE(int x, int y, double value) {
            elements[y * 2 + x] = value;
        }

	// Gets an element, based on 2D-coords.
        public double getE(int x, int y) {
            return elements[y * 2 + x];
        }

	// Performs vector-matrix multiplication with a 2D double
	// vector.
        private static Vector2d multByVec(Vector2d vec, Matrix2x2d mat) {
            return new Vector2d(
                mat.getE(0, 0) * vec.X + mat.getE(1,0) * vec.Y,
                mat.getE(0, 1) * vec.X + mat.getE(1,1) * vec.Y
            );
        }

	// Gets the inverse matrix.
        public Matrix2x2d getInverse() {
	    // The determinant of the matrix is first generated, by
	    // subtracting the product of the top-right and
	    // bottom-left from the product of the top-left and
	    // bottom-right.
	    //
	    // TL = top-left; TR = top-right; BL = bottom-left; BR = bottom-right;
	    // determinant = (TL * BR) - (TR * BL)
            double determinant = 1/(getE(0,0) * getE(1,1) - getE(1,0) * getE(0,1));

	    // The inverse matrix, to be multiplied by the determinant
	    // is made, by taking the matrix, swapping TL and BR and
	    // multiplying TR and BL by -1 (negative one).
            Matrix2x2d invMat = new Matrix2x2d(new double[] { getE(1,1), -getE(1,0),
                                                             -getE(0, 1), getE(0,0)});

	    // The inverse matrix, multiplied by determinant is
	    // returned.
            return invMat * determinant;
        }

	// Multiplies all elements in the matrix by a scalar.
        private static Matrix2x2d multByScalar(double scalar, Matrix2x2d mat) {
            double[] newElements = new double[4];
            for(int i = 0; i < mat.elements.Length; i++) {
                newElements[i] = mat.elements[i] * scalar;
            }
            return new Matrix2x2d(newElements);
        }

	// Operator-overloading for multiplying with a scalar on the lhs.
        public static Matrix2x2d operator *(double scalar, Matrix2x2d mat) {
            return multByScalar(scalar, mat);

        }
	// Operator-overloading for multiplying with a scalar on the rhs.
        public static Matrix2x2d operator *(Matrix2x2d mat, double scalar) {
            return multByScalar(scalar, mat);
        }

	// Operator-overloading for multiplying with a vector on the lhs.
        public static Vector2d operator *(Vector2d vec, Matrix2x2d mat) {
            return multByVec(vec, mat);

        }
	// Operator-overloading for multiplying with a vector on the rhs.
        public static Vector2d operator *(Matrix2x2d mat, Vector2d vec) {
            return multByVec(vec, mat);
        }
    }
}
