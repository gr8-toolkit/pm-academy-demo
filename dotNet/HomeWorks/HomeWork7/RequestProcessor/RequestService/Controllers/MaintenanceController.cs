using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using RequestService.Models;

namespace RequestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MaintenanceController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public MaintenanceController(IMemoryCache cache, ILogger<MaintenanceController> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpPost("storage/seed")]
        public IActionResult Seed()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            _cache.Set(GetFeedbackKey(1), new Feedback
            {
                Rate = 1,
                Message = "Preset feedback #1"
            }, cacheEntryOptions);
            _cache.Set(GetFeedbackKey(2), new Feedback
            {
                Rate = 2,
                Message = "Preset feedback #2"
            }, cacheEntryOptions);
            
            return Ok();
        }


        [HttpDelete("storage")]
        public IActionResult Delete()
        {
            var cache = (MemoryCache) _cache;
            cache.Compact(1d);
            return NoContent();
        }

        private static string GetFeedbackKey(int id) => $"feedback_{id}";
    }
}
