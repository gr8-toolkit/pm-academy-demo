using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Services
{
    public interface IDisposableService : IDisposable { }

    public interface IDisposableServiceM1 : IDisposableService { }

    public interface IDisposableServiceM2 : IDisposableService { }

    public interface IDisposableServiceM3 : IDisposableService { }
}
