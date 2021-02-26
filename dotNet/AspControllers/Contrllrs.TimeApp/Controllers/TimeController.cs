using Microsoft.AspNetCore.Mvc;
using Contrllrs.TimeApp.Services;

namespace Contrllrs.TimeApp.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")] 
    public class TimeController : ControllerBase
    {
        private readonly ITimeService _timeService;
        
        public TimeController()
        {
            // ToDo: Use DI instead of manual service creation
            _timeService = new TimeService();
        }

        [HttpGet]
        public string Get()
        {
            return _timeService.GetServerTime();
        }
    }
}
