using System;

namespace BranchesApp.Example9
{
    public class Program
    {
        public abstract class Shape
        {
            public double Square { get; set; }
        }

        public class Triangle : Shape
        {
        }

        public class Rect : Shape
        {
        }

        public class Line
        {
            public double Length { get; set; }
        }

        static void Main()
        {
            PrintGeometry(new Rect {Square = 1000d});
            PrintGeometry2(new Rect {Square = -42d});
        }

        private static void PrintGeometry(object geometry)
        {
            switch (geometry)
            {
                case Shape shape : 
                    Console.WriteLine("Shape is {0} m2", shape.Square);
                    break;
                case Line line:
                    Console.WriteLine("Line is {0} m", line.Length);
                    break;
                default:  throw new ArgumentOutOfRangeException();
            }
        }

        private static void PrintGeometry2(object geometry)
        {
            switch (geometry)
            {
                case Shape shape when shape.Square < 0:
                    Console.WriteLine("Shape is invalid");
                    break;
                case Shape shape:
                    Console.WriteLine("Shape is {0} m2", shape.Square);
                    break;
                case Line line:
                    Console.WriteLine("Line is {0} m", line.Length);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
