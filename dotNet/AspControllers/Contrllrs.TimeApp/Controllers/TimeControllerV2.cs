using System;
using Microsoft.AspNetCore.Mvc;
using Contrllrs.TimeApp.Services;
using Microsoft.Extensions.Logging;

namespace Contrllrs.TimeApp.Controllers
{
    [ApiController]
    [Route("api/v2/time")]
    public class TimeControllerV2 : ControllerBase
    {
        private readonly ITimeService _timeService;
        private readonly ILogger<TimeControllerV2> _logger;

        public TimeControllerV2(ITimeService timeService, ILogger<TimeControllerV2> logger)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public string Get()
        {
            // Using of injected services :
            _logger.LogInformation($"Incoming request : {HttpContext.Request.Path}");
            return _timeService.GetServerTime();
        }
    }
}
