using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GenericHost.HostedServices
{
    class LastHostedService : IHostedService
    {
        private readonly ILogger<LastHostedService> _logger;

        public LastHostedService(ILogger<LastHostedService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Last start");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Last stop");
        }
    }
}