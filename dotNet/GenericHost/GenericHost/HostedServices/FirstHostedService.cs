using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost.HostedServices
{
    class FirstHostedService : IHostedService
    {
        private readonly ILogger<FirstHostedService> _logger;

        public FirstHostedService(ILogger<FirstHostedService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("First start");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("First stop");
        }
    }
}