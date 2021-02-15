using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using GenericHost.ApplicationServices;
using GenericHost.Models;
using GenericHost.Models.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GenericHost.HostedServices
{
    class CPULoadHostedService : BackgroundService
    {
        private readonly PrimesFinder _primesFinder;
        private readonly JsonWorker _jsonWorker;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILogger<CPULoadHostedService> _logger;
        private readonly Settings _settings;
        private Timer _timer;

        public CPULoadHostedService(
            PrimesFinder primesFinder,
            JsonWorker jsonWorker,
            IHostApplicationLifetime hostApplicationLifetime,
            IOptionsSnapshot<Settings> settingsSnapshot,
            ILogger<CPULoadHostedService> logger)
        {
            _primesFinder = primesFinder ?? throw new ArgumentNullException(nameof(primesFinder));
            _jsonWorker = jsonWorker;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
            _settings = settingsSnapshot?.Value ?? throw new ArgumentNullException(nameof(settingsSnapshot));
            _timer = new Timer(Callback, null, System.Threading.Timeout.InfiniteTimeSpan, System.Threading.Timeout.InfiniteTimeSpan);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer.Change(TimeSpan.FromSeconds(5), System.Threading.Timeout.InfiniteTimeSpan);
        }

        private void Callback(object state)
        {
            _logger.LogInformation("Start load");

            Stopwatch stopwatch = Stopwatch.StartNew();
            int[] primes = _primesFinder.FindPrimes(_settings);
            stopwatch.Stop();

            _logger.LogInformation("Load finish. Time: {Time}", stopwatch.Elapsed);

            _jsonWorker.WriteResult(new Result(true, stopwatch.Elapsed.ToString(), primes));

            _hostApplicationLifetime.StopApplication();
        }
    }
}
