using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using AuthWebApps.CustomScheme.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace AuthWebApps.CustomScheme.Controllers
{
    [ApiController]
    [Route("api/v2/forecast")]
    [Authorize]
    public class WeatherForecastController2 : ControllerBase
    {
        private static readonly string[] Summaries = 
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger _logger;

        public WeatherForecastController2(ILogger<WeatherForecastController2> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            if (!(HttpContext.User.Identity is AccountIdentity identity)) return Unauthorized();
            
            // Accounting
            _logger.LogInformation($"User {identity.AccountId} in role {identity.Role} requests forecast.");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
