using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjectionExampleApp.Services
{
    public class DisposableServiceM2 : IDisposableServiceM2
    {
        private ILogger<DisposableServiceM2> _logger;

        public DisposableServiceM2(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DisposableServiceM2>();
        }

        public void Dispose()
        {
            _logger.LogInformation("Calls dispose method on DisposableServiceM2");
        }
    }
}
