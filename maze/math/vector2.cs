using System;
using System.Drawing;

namespace textured_raycast.maze.math
{
    // Vector 2 double.
    class Vector2<T> {
        // Holds x and y of vector.
        public T x, y;

        public Vector2(T x, T y) {
            this.x = x;
            this.y = y;
        }

        // Bunch of operator overloading, making vector math easier.
        // Equality operator: Checks if two vectors are equal.
        public static bool operator ==(Vector2<T> vec1, Vector2<T> vec2) {
            return ((dynamic)vec1.x == (dynamic)vec2.x) && ((dynamic)vec1.y == (dynamic)vec2.y);
        }

        // Inequality operator: Checks if two vectors are /not/ equal
        public static bool operator !=(Vector2<T> vec1, Vector2<T> vec2) {
            return !(vec1 == vec2);
        }

        // Addition operator: Adds X to X and Y to Y
        public static Vector2<T> operator +(Vector2<T> vec1, Vector2<T> vec2) {
            return new Vector2<T>((dynamic)vec1.x + (dynamic)vec2.x, (dynamic)vec1.y + (dynamic)vec2.y);
        }
        // Subtraction operator: Subtracts X from X and Y from Y
        public static Vector2<T> operator -(Vector2<T> vec1, Vector2<T> vec2) {
            return new Vector2<T>((dynamic)vec1.x - (dynamic)vec2.x, (dynamic)vec1.y - (dynamic)vec2.y);
        }
        // Multiplication operator: Multiplies vector by scalar.
        public static Vector2<T> operator *(Vector2<T> vec1, double scalar) {
            return new Vector2<T>((dynamic)vec1.x * scalar, (dynamic)vec1.y * scalar);
        }
        // Multiplication operator #2: Allows for scalar to be first.
        public static Vector2<T> operator *(double scalar, Vector2<T> vec1) {
            return new Vector2<T>((dynamic)vec1.x * scalar, (dynamic)vec1.y * scalar);
        }
        // Multiplication operator #3: Multiply the components of two vectors.
        public static Vector2<T> operator *(Vector2<T> vec1, Vector2<T> vec2) {
            return new Vector2<T>((dynamic)vec1.x * (dynamic)vec2.x, (dynamic)vec1.y * (dynamic)vec2.y);
        }
        // Division operator: Divides vector by scalar.
        public static Vector2<T> operator /(Vector2<T> vec1, double scalar) {
            return new Vector2<T>((dynamic)vec1.x / scalar, (dynamic)vec1.y / scalar);
        }

        // Non-static function. Floors vector, basically turning it into an
        // integer vector.
        public Vector2<int> Floor(){
            return new Vector2<int>(Math.Floor((dynamic)x), Math.Floor((dynamic)y));
        }
        public double DistTo(Vector2<T> otherVec2d){
            return Math.Sqrt(Math.Pow((dynamic)otherVec2d.x - (dynamic)x, 2) + Math.Pow((dynamic)otherVec2d.y - (dynamic)y, 2));
        }

        // Following function tests the functionality of the operator overloading.
        // Should say true at the end of all the lines.
        public static void test() {
            Console.WriteLine($"Equals: {new Vector2<double>(5, 7) == new Vector2<double>(5, 7)}");

            Console.Read();
        }
    }
}
