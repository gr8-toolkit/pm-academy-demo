using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Services
{
    public class OperationExample : IOperationTransient, IOperationScoped, IOperationSingleton
    {
        private int _state;

        public OperationExample()
        {
            _state = 0;
        }

        public int State => ++_state;
    }
}
