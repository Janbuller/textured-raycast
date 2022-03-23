using System;
using System.Collections.Generic;

namespace textured_raycast.maze.math
{
    // Vector 2 double.
    class Vector2i {
        // Holds x and y of vector.
        public int x, y;

        public Vector2i(int x, int y) {
            this.x = x;
            this.y = y;
        }
        public static bool Equals(Vector2i vec1, Vector2i vec2)
        {
            if (ReferenceEquals(vec1, vec2)) return true;
            if (ReferenceEquals(vec1, null)) return false;
            if (ReferenceEquals(vec2, null)) return false;
            if (vec1.GetType() != vec2.GetType()) return false;
            return vec1.x == vec2.x && vec1.y == vec2.y;
        }

        public static int GetHashCode(Vector2i obj)
        {
            return HashCode.Combine(obj.x, obj.y);
        }

        public override bool Equals(object vec)
        {
            return Vector2i.Equals(this, vec);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
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
            return new Vector2i(vec1.x + vec2.x, vec1.y + vec2.y);
        }
        // Subtraction operator: Subtracts X from X and Y from Y
        public static Vector2i operator -(Vector2i vec1, Vector2i vec2) {
            return new Vector2i(vec1.x - vec2.x, vec1.y - vec2.y);
        }
        // Multiplication operator: Multiplies vector by scalar.
        public static Vector2i operator *(Vector2i vec1, double scalar) {
            return new Vector2i(Convert.ToInt32(vec1.x * scalar), Convert.ToInt32(vec1.y * scalar));
        }
        public static Vector2i operator *(Vector2i vec1, int scalar) {
            return new Vector2i(vec1.x * scalar, vec1.y * scalar);
        }
        // Multiplication operator #2: Allows for scalar to be first.
        public static Vector2i operator *(double scalar, Vector2i vec1) {
            return new Vector2i(Convert.ToInt32(vec1.x * scalar), Convert.ToInt32(vec1.y * scalar));
        }
        public static Vector2i operator *(int scalar, Vector2i vec1) {
            return new Vector2i(vec1.x * scalar, vec1.y * scalar);
        }
        // Multiplication operator #3: Multiply the components of two vectors.
        public static Vector2i operator *(Vector2i vec1, Vector2i vec2) {
            return new Vector2i(vec1.x * vec2.x, vec1.y * vec2.y);
        }
        // Division operator: Divides vector by scalar.
        public static Vector2i operator /(Vector2i vec1, double scalar) {
            return new Vector2i(Convert.ToInt32(vec1.x / scalar), Convert.ToInt32(vec1.y / scalar));
        }
        public static Vector2i operator /(Vector2i vec1, int scalar) {
            return new Vector2i(vec1.x / scalar, vec1.y / scalar);
        }

        // allow casting v2d to v2i
        public static explicit operator Vector2i(Vector2d vec) {
            return new Vector2i(Convert.ToInt32(vec.x), Convert.ToInt32(vec.y));
        }

        // Following function tests )the functionality of the operator overloading.
        // Should say true at the end of all the lines.
        public static void test() {
            // Console.WriteLine($"Equals: {new Vector2i(5, 7) == new Vector2i(5, 7)}");
            // Console.WriteLine($"EqualsInv: {!(new Vector2i(2, 7) == new Vector2i(5, 7))}");
            // Console.WriteLine($"NotEquals: {new Vector2i(2, 7) != new Vector2i(5, 7)}");
            // Console.WriteLine($"NotEqualsInv: {!(new Vector2i(5, 7) != new Vector2i(5, 7))}");
            // Console.WriteLine("");

            // Console.WriteLine($"Add: {(new Vector2i(9, 2) + new Vector2i(-3, 7)) == new Vector2i(6, 9)}");
            // Console.WriteLine($"Sub: {(new Vector2i(9, 2) - new Vector2i(-3, 7)) == new Vector2i(12, -5)}");
            // Console.WriteLine($"Mult: {(new Vector2i(9, 2) * 2.2) == new Vector2i(19.8, 4.4)}");
            // Console.WriteLine($"Mult2: {(2.2 * new Vector2i(9, 2)) == new Vector2i(19.8, 4.4)}");
            // Console.WriteLine($"Div: {(new Vector2i(9, 2) / 20) == new Vector2i(0.45, 0.1)}");

            // Console.Read();
        }
    }
}
