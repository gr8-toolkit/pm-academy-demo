using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApps.AuthServices;

namespace AuthWebApps.CustomScheme.Controllers
{
    [ApiController]
    [Route("api/v1/forecast")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = 
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ISessionService<object> _sessions;

        [FromHeader(Name = "Authorization")]
        public string Authorization { get; set; }

        public WeatherForecastController(ISessionService<object> sessions)
        {
            _sessions = sessions;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WeatherForecast>>> Get()
        {
            // Demo note: Authorization logic without framework tools
            
            if (string.IsNullOrEmpty(Authorization)) return Unauthorized();
            var session = await _sessions.GetSessionAsync(Authorization, null, null);
            if (session == null) return Unauthorized();
            
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
