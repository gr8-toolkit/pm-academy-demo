namespace DesignPatterns.IoC
{
    public struct StructSingleton
    {
        public StructSingleton(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}