using System;

namespace textured_raycast.maze.math
{
    // Vector 2 double.
    class Vector2d {
        // Holds x and y of vector.
        public double x, y;

        public Vector2d(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public Vector2d(Vector2d vec) {
            this.x = vec.x;
            this.y = vec.y;
        }
        public static bool Equals(Vector2d vec1, Vector2d vec2)
        {
            if (ReferenceEquals(vec1, vec2)) return true;
            if (ReferenceEquals(vec1, null)) return false;
            if (ReferenceEquals(vec2, null)) return false;
            if (vec1.GetType() != vec2.GetType()) return false;
            return vec1.x == vec2.x && vec1.y == vec2.y;
        }

        public static int GetHashCode(Vector2d obj)
        {
            return HashCode.Combine(obj.x, obj.y);
        }

        public override bool Equals(object vec)
        {
            return Vector2d.Equals(this, vec);
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }
        
        // Bunch of operator overloading, making vector math easier.
        // Equality operator: Checks if two vectors are equal.
        public static bool operator ==(Vector2d vec1, Vector2d vec2)
        {
            return Vector2d.Equals(vec1, vec2);
        }

        // Inequality operator: Checks if two vectors are /not/ equal
        public static bool operator !=(Vector2d vec1, Vector2d vec2) {
            return !Vector2d.Equals(vec1, vec2);
        }

        // Addition operator: Adds X to X and Y to Y
        public static Vector2d operator +(Vector2d vec1, Vector2d vec2) {
            return new Vector2d(vec1.x + vec2.x, vec1.y + vec2.y);
        }
        // Subtraction operator: Subtracts X from X and Y from Y
        public static Vector2d operator -(Vector2d vec1, Vector2d vec2) {
            return new Vector2d(vec1.x - vec2.x, vec1.y - vec2.y);
        }
        // Multiplication operator: Multiplies vector by scalar.
        public static Vector2d operator *(Vector2d vec1, double scalar) {
            return new Vector2d(vec1.x * scalar, vec1.y * scalar);
        }
        // Multiplication operator #2: Allows for scalar to be first.
        public static Vector2d operator *(double scalar, Vector2d vec1) {
            return new Vector2d(vec1.x * scalar, vec1.y * scalar);
        }
        // Multiplication operator #3: Multiply the components of two vectors.
        public static Vector2d operator *(Vector2d vec1, Vector2d vec2) {
            return new Vector2d(vec1.x * vec2.x, vec1.y * vec2.y);
        }
        // Division operator: Divides vector by scalar.
        public static Vector2d operator /(Vector2d vec1, double scalar) {
            return new Vector2d(vec1.x / scalar, vec1.y / scalar);
        }

        // allow casting v2i to v2d
        public static explicit operator Vector2d(Vector2i vec) {
            return new Vector2d(vec.x, vec.y);
        }

        // Non-static function. Floors vector, basically turning it into an
        // integer vector.
        public Vector2d Floor()
        {
            return new Vector2d(Math.Floor(x), Math.Floor(y));
        }
        public void Normalize()
        {
            double dist = DistTo(new Vector2d(0, 0));

            x /= dist;
            y /= dist;
        }

        // Using multiplication instead of Math.Pow for this is /literally/ over
        // 10 times faster.
        //
        // Doing this 1.000.000 times takes:
        // Math.Pow():     ~50-60ms
        // Multiplication: ~0-5ms
        public double DistTo(Vector2d otherVec2d){
            double x1 = otherVec2d.x - x;
            double y1 = otherVec2d.y - y;
            return Math.Sqrt(x1*x1 + y1*y1);
        }

        // Following function tests the functionality of the operator overloading.
        // Should say true at the end of all the lines.
        public static void test() {
            Console.WriteLine($"Equals: {new Vector2d(5, 7) == new Vector2d(5, 7)}");
            Console.WriteLine($"EqualsInv: {!(new Vector2d(2, 7) == new Vector2d(5, 7))}");
            Console.WriteLine($"NotEquals: {new Vector2d(2, 7) != new Vector2d(5, 7)}");
            Console.WriteLine($"NotEqualsInv: {!(new Vector2d(5, 7) != new Vector2d(5, 7))}");
            Console.WriteLine("");

            Console.WriteLine($"Add: {(new Vector2d(9, 2) + new Vector2d(-3, 7)) == new Vector2d(6, 9)}");
            Console.WriteLine($"Sub: {(new Vector2d(9, 2) - new Vector2d(-3, 7)) == new Vector2d(12, -5)}");
            Console.WriteLine($"Mult: {(new Vector2d(9, 2) * 2.2) == new Vector2d(19.8, 4.4)}");
            Console.WriteLine($"Mult2: {(2.2 * new Vector2d(9, 2)) == new Vector2d(19.8, 4.4)}");
            Console.WriteLine($"Div: {(new Vector2d(9, 2) / 20) == new Vector2d(0.45, 0.1)}");

            Console.Read();
        }
    }
}
