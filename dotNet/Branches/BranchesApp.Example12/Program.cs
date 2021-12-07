using System;

namespace BranchesApp.Example12
{
    public abstract class Shape
    {
        public int Area { get; internal set; }

        public abstract bool IsBig();
    }

    public class Rect : Shape
    {
        public override bool IsBig() => Area >= 100;

        public Rect(int area)
        {
            Area = area;
        }
    }

    public class Triangle : Shape
    {
        public override bool IsBig() => Area >= 50;

        public Triangle(int area)
        {
            Area = area;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Shape shape = new Triangle(60);

            if (shape.IsBig())
            {
                Console.WriteLine("My shape is big");
            }
            else
            {
                Console.WriteLine("My shape is small");
            }
        }
    }
}
