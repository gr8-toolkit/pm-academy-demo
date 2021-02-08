using System;

namespace DesignPatterns.IoC
{
    public class ServiceCollection : IServiceCollection
    {
        public IServiceCollection AddTransient<T>()
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddTransient<T>(Func<T> factory)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddTransient<T>(Func<IServiceProvider, T> factory)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddSingleton<T>()
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddSingleton<T>(T service)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddSingleton<T>(Func<T> factory)
        {
            throw new NotImplementedException();
        }

        public IServiceCollection AddSingleton<T>(Func<IServiceProvider, T> factory)
        {
            throw new NotImplementedException();
        }

        public IServiceProvider BuildServiceProvider()
        {
            throw new NotImplementedException();
        }
    }
}