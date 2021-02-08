using System;

namespace DesignPatterns.IoC
{
    public interface IServiceCollection
    {
        IServiceCollection AddTransient<T>();

        IServiceCollection AddTransient<T>(Func<T> factory);

        IServiceCollection AddTransient<T>(Func<IServiceProvider, T> factory);

        IServiceCollection AddSingleton<T>();

        IServiceCollection AddSingleton<T>(T service);

        IServiceCollection AddSingleton<T>(Func<T> factory);

        IServiceCollection AddSingleton<T>(Func<IServiceProvider, T> factory);

        IServiceProvider BuildServiceProvider();
    }
}