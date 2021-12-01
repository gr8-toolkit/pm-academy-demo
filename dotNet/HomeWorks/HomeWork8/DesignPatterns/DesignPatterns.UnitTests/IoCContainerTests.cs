using System;
using System.Net.NetworkInformation;
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
            Assert.Same(second, first);
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
            Assert.Same(second, first);
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
            Assert.Same(second, first);
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
            Assert.Same(second, first);
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
            Assert.NotSame(second, first);
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
            Assert.NotSame(second, first);
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
            Assert.NotSame(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void ComplexTest()
        {
            _services.AddTransient(provider => new SomeSecondTransient(provider.GetService<SomeSingleton>()));
            _services.AddSingleton<SomeSingleton>();

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

        [Fact]
        public void DifferentRegistrationsTest()
        {
            _services.AddSingleton<SomeSingleton>();
            _services.AddTransient<SomeSingleton>();
            _services.AddSingleton<SomeSingleton>();

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Same(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void DuplicateRegistrationsTest()
        {
            _services.AddSingleton<SomeSingleton>();
            _services.AddSingleton<SomeSingleton>();
            _services.AddSingleton<SomeSingleton>();
            _services.AddSingleton<SomeSingleton>();

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Same(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void LastRegistrationUsedTest()
        {
            _services.AddSingleton<SomeSingleton>(() => throw new InvalidOperationException());
            _services.AddSingleton<SomeSingleton>(() => throw new ArgumentNullException());
            _services.AddSingleton<SomeSingleton>();

            IServiceProvider provider = _services.BuildServiceProvider();

            SomeSingleton first = provider.GetService<SomeSingleton>();
            SomeSingleton second = provider.GetService<SomeSingleton>();

            Assert.Equal(second, first);
            Assert.Same(second, first);
            Assert.Equal(1, first.Counter);
            Assert.Equal(1, second.Counter);
        }

        [Fact]
        public void ExceptionFactoryTest()
        {
            _services.AddSingleton<SomeSingleton>(p => throw new NetworkInformationException());

            IServiceProvider provider = _services.BuildServiceProvider();

            Assert.Throws<NetworkInformationException>(() => provider.GetService<SomeSingleton>());
        }

        [Fact]
        public void ExceptionFactory1Test()
        {
            _services.AddSingleton<SomeSingleton>(() => throw new NetworkInformationException());

            IServiceProvider provider = _services.BuildServiceProvider();

            Assert.Throws<NetworkInformationException>(() => provider.GetService<SomeSingleton>());
        }

        [Fact]
        public void ExceptionFactory2Test()
        {
            _services.AddTransient<SomeTransient>(() => throw new NetworkInformationException());

            IServiceProvider provider = _services.BuildServiceProvider();

            Assert.Throws<NetworkInformationException>(() => provider.GetService<SomeSingleton>());
        }

        [Fact]
        public void ExceptionFactory3Test()
        {
            _services.AddTransient<SomeTransient>(p => throw new NetworkInformationException());

            IServiceProvider provider = _services.BuildServiceProvider();

            Assert.Throws<NetworkInformationException>(() => provider.GetService<SomeSingleton>());
        }

        [Fact]
        public void OnAddExceptionsTest()
        {
            _services.AddSingleton<BrokenType>();
            _services.AddSingleton<SomeSingleton>(() => throw new NetworkInformationException());
            _services.AddSingleton<SomeSingleton>(p => throw new NetworkInformationException());
            _services.AddTransient<BrokenType>();
            _services.AddTransient<SomeTransient>(() => throw new NetworkInformationException());
            _services.AddTransient<SomeTransient>(p => throw new NetworkInformationException());

            Assert.True(true);
        }

        [Fact]
        public void AddSingletonStructTest()
        {
            _services.AddSingleton<StructSingleton>(new StructSingleton(1234567));

            IServiceProvider provider = _services.BuildServiceProvider();

            StructSingleton first = provider.GetService<StructSingleton>();
            StructSingleton second = provider.GetService<StructSingleton>();

            Assert.Equal(second, first);
            Assert.Equal(1234567, first.Value);
        }

        [Fact]
        public void AddSingletonRecordTest()
        {
            _services.AddSingleton<RecordSingleton>(new RecordSingleton(987654));

            IServiceProvider provider = _services.BuildServiceProvider();

            RecordSingleton first = provider.GetService<RecordSingleton>();
            RecordSingleton second = provider.GetService<RecordSingleton>();

            Assert.Same(second, first);
            Assert.Equal(second, first);
            Assert.Equal(987654, first.SomeValue);
        }

        [Fact]
        public void MultiBuildTest()
        {
            _services.AddTransient(provider => new SomeSecondTransient(provider.GetService<SomeSingleton>()));
            _services.AddSingleton<SomeSingleton>();

            IServiceProvider serviceProvider1 = _services.BuildServiceProvider();
            IServiceProvider serviceProvider2 = _services.BuildServiceProvider();

            Assert.NotSame(serviceProvider1, serviceProvider2);
        }
    }
}