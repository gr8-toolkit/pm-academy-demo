using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Services
{
    public class DisposableServiceM1 : IDisposableServiceM1
    {
        private ILogger<DisposableServiceM1> _logger;

        public DisposableServiceM1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DisposableServiceM1>();
        }

        public void Dispose()
        {
            _logger.LogInformation("Calls dispose method on DisposableServiceM1");
        }
    }
}
