using System;

namespace Common
{
    public class ClassExample<T> : IExample<T>
    {
        public T Data;

        public ClassExample(T data)
        {
            Data = data;
        }
    }

    public readonly struct StructExample<T> : IExample<T>
    {
        public readonly T Data;

        public StructExample(T d)
        {
            Data = d;
        }
    }

    public static class GenericExample
    {
        public static void Foo()
        {

        }

        public static string Format<T>(T data) =>
            $"Result: {data.ToString()}";
    }
}
