using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// based on https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#service-lifetimes
namespace DependencyInjectionExampleApp.Services
{

    public interface IOperation
    {
        int State { get; }
    }

    public interface IOperationTransient : IOperation { }

    public interface IOperationSingleton : IOperation { }

    public interface IOperationScoped : IOperation { }
}
