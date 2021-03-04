using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DependencyInjectionExampleApp.Services
{
    public class DisposableServiceM3 : IDisposableServiceM3
    {
        private ILogger<DisposableServiceM3> _logger;

        public DisposableServiceM3(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DisposableServiceM3>();
        }

        public void Dispose()
        {
            _logger.LogInformation("Calls dispose method on DisposableServiceM3");
        }
    }
}
