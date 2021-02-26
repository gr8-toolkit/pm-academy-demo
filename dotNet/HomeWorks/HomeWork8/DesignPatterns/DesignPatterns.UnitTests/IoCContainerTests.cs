using DesignPatterns.IoC;
using Xunit;
using IServiceProvider = DesignPatterns.IoC.IServiceProvider;

namespace DesignPatterns.UnitTests
{
    public class IoCContainerTests
    {
        private readonly IServiceCollection _services;

        public IoCContainerTests()
        {
            _services = new ServiceCollection();
        }

        [Fact]
        public void AddSingleton1Test()
        {
            _services.AddSingleton<SomeSingleton>();

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddSingleton2Test()
        {
            _services.AddSingleton(new SomeSingleton());

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddSingleton3Test()
        {
            _services.AddSingleton(() => new SomeSingleton());

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddSingleton4Test()
        {
            _services.AddSingleton(provider => new SomeSingleton());

            IServiceProvider serviceProvider = _services.BuildServiceProvider();

            SomeSingleton first = serviceProvider.GetService<SomeSingleton>();
            SomeSingleton second = serviceProvider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddTransient1Test()
        {
            _services.AddTransient<SomeTransient>();

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeTransient first = provider.GetService<SomeTransient>();
            SomeTransient second = provider.GetService<SomeTransient>();

            Assert.NotEqual(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddTransient2Test()
        {
            _services.AddTransient<SomeTransient>(() => new SomeTransient());

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeTransient first = provider.GetService<SomeTransient>();
            SomeTransient second = provider.GetService<SomeTransient>();

            Assert.NotEqual(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void AddTransient3Test()
        {
            _services.AddTransient(provider => new SomeTransient());

            IServiceProvider serviceProvider = _services.BuildServiceProvider();

            SomeTransient first = serviceProvider.GetService<SomeTransient>();
            SomeTransient second = serviceProvider.GetService<SomeTransient>();

            Assert.NotEqual(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void ComplexTest()
        {
            _services.AddSingleton<SomeSingleton>();
            _services.AddTransient(provider => new SomeSecondTransient(provider.GetService<SomeSingleton>()));

            IServiceProvider serviceProvider = _services.BuildServiceProvider();

            SomeSingleton sing = serviceProvider.GetService<SomeSingleton>();
            SomeSecondTransient first = serviceProvider.GetService<SomeSecondTransient>();
            SomeSecondTransient second = serviceProvider.GetService<SomeSecondTransient>();

            Assert.NotEqual(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
            Assert.Equal(1, sing.Counter);
        }

        [Fact]
        public void NoServiceTest()
        {
            IServiceProvider serviceProvider = _services.BuildServiceProvider();

            SomeSingleton singleton = serviceProvider.GetService<SomeSingleton>();
            SomeSecondTransient someSecondTransient = serviceProvider.GetService<SomeSecondTransient>();

            Assert.Null(singleton);
            Assert.Null(someSecondTransient);
        }
    }
}