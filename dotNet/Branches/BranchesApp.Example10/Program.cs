using System;

namespace BranchesApp.Example10
{
    public class Program
    {
        public abstract class Shape
        {
            public double Area { get; set; }
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
            PrintNumber(4);
            PrintGeometry(new Rect { Area = 1000d });
            PrintGeometry2(new Rect { Area = 42d });
        }

        private static void PrintNumber(int number)
        {
            var someValue = number switch
            {
                1 => "1",
                >= 2 and <= 5 => "More or equal than 2 and less or equal than 5",
                _ => "Unknown value"
            };

            Console.WriteLine("Value is {0}", someValue);
        }

        private static void PrintGeometry(object geometry)
        {
            var someValue = geometry switch
            {
                Shape shape => $"Shape is {shape.Area} m2",
                Line line => $"Line is {line.Length} m",
                _ => "Unknown value"
            };

            Console.WriteLine("Value is {0}", someValue);
        }

        private static void PrintGeometry2(Shape shape)
        {
            var someValue = shape switch
            {
                Triangle { Area: > 100 } triangle => $"Big triangle is {triangle.Area}",
                Triangle triangle => $"Triangle is {triangle.Area}",
                Rect rect when rect.Area > 100 => $"Big rect is {rect.Area}",
                Rect rect => $"Rect is {rect.Area}",
                { Area: 100 } => "Area is 100",
                _ => "Unknown value"
            };

            Console.WriteLine("Value is {0}", someValue);
        }
    }
}
