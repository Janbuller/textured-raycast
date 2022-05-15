using System;
using System.Collections.Generic;

namespace textured_raycast.maze.math
{
    // Vector 2 double.
    public class Vector2i {
        // Holds x and y of vector.
        private int x, y;

	// Uses parameters, to allow for different names for the same
	// variables.
        public int X { get { return x; } set {x = value; } }
        public int Y { get { return y; } set {y = value; } }
	public int Width { get {return X; } set { X = value; } }
	public int Height{ get {return Y; } set { Y = value; } }

        public Vector2i(int x, int y) {
            this.X = x;
            this.Y = y;
        }

	// Checks for equality, returning false if either is null.
        public static bool Equals(Vector2i vec1, Vector2i vec2)
        {
            if (ReferenceEquals(vec1, vec2)) return true;
            if (ReferenceEquals(vec1, null)) return false;
            if (ReferenceEquals(vec2, null)) return false;
            if (vec1.GetType() != vec2.GetType()) return false;
            return vec1.X == vec2.X && vec1.Y == vec2.Y;
        }

        public static int GetHashCode(Vector2i obj)
        {
            return obj.GetHashCode();
        }

	// Dotnet /really/ wants this Equals function, for some
	// reason.
        public override bool Equals(object vec)
        {
            return Vector2i.Equals(this, vec);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
        
        // Bunch of operator overloading, making vector math easier.
        // Equality operator: Checks if two vectors are equal.
        public static bool operator ==(Vector2i vec1, Vector2i vec2)
        {
            return Vector2i.Equals(vec1, vec2);
        }

        // Inequality operator: Checks if two vectors are /not/ equal
        public static bool operator !=(Vector2i vec1, Vector2i vec2) {
            return !Vector2i.Equals(vec1, vec2);
        }

        // Addition operator: Adds X to X and Y to Y
        public static Vector2i operator +(Vector2i vec1, Vector2i vec2) {
            return new Vector2i(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }
        // Subtraction operator: Subtracts X from X and Y from Y
        public static Vector2i operator -(Vector2i vec1, Vector2i vec2) {
            return new Vector2i(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }
        // Multiplication operator: Multiplies vector by scalar.
        public static Vector2i operator *(Vector2i vec1, double scalar) {
            return new Vector2i(Convert.ToInt32(vec1.X * scalar), Convert.ToInt32(vec1.Y * scalar));
        }
        public static Vector2i operator *(Vector2i vec1, int scalar) {
            return new Vector2i(vec1.X * scalar, vec1.Y * scalar);
        }
        // Multiplication operator #2: Allows for scalar to be first.
        public static Vector2i operator *(double scalar, Vector2i vec1) {
            return new Vector2i(Convert.ToInt32(vec1.X * scalar), Convert.ToInt32(vec1.Y * scalar));
        }
        public static Vector2i operator *(int scalar, Vector2i vec1) {
            return new Vector2i(vec1.X * scalar, vec1.Y * scalar);
        }
        // Multiplication operator #3: Multiply the components of two vectors.
        public static Vector2i operator *(Vector2i vec1, Vector2i vec2) {
            return new Vector2i(vec1.X * vec2.X, vec1.Y * vec2.Y);
        }
        // Division operator: Divides vector by scalar.
        public static Vector2i operator /(Vector2i vec1, double scalar) {
            return new Vector2i(Convert.ToInt32(vec1.X / scalar), Convert.ToInt32(vec1.Y / scalar));
        }
        public static Vector2i operator /(Vector2i vec1, int scalar) {
            return new Vector2i(vec1.X / scalar, vec1.Y / scalar);
        }

        // allow casting v2d to v2i
        public static explicit operator Vector2i(Vector2d vec) {
            return new Vector2i(Convert.ToInt32(vec.X), Convert.ToInt32(vec.Y));
        }
    }
}
