namespace DesignPatterns.IoC
{
    public interface IServiceProvider
    {
        T GetService<T>();
    }
}