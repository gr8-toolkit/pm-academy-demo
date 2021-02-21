namespace DesignPatterns.IoC
{
    public record RecordSingleton(int SomeValue)
    {
        public int SomeValue { get; } = SomeValue;
    }
}