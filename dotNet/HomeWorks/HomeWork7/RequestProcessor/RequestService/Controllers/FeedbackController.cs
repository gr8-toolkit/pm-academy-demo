using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using RequestService.Models;

namespace RequestService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMemoryCache _cache;

        public FeedbackController(IMemoryCache cache, ILogger<FeedbackController> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public ActionResult<Feedback> Get(int id)
        {
            if (!_cache.TryGetValue(GetFeedbackKey(id), out var feedback))
            {
                return NotFound();
            }

            return (Feedback)feedback;
        }

        [HttpPost("{id}")]
        public IActionResult Post(int id, Feedback feedback)
        {
            if (!ModelState.IsValid) return BadRequest("Model is invalid");
            if (_cache.TryGetValue(GetFeedbackKey(id), out _))
            {
                return Conflict("Already exists");
            }
            
            var cacheEntryOptions = new MemoryCacheEntryOptions();
            _cache.Set(GetFeedbackKey(id), feedback, cacheEntryOptions);

            return CreatedAtRoute("", new {id});
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_cache.TryGetValue(GetFeedbackKey(id), out var feedback))
            {
                return NotFound("Feedback was not found");
            }
            _cache.Remove(GetFeedbackKey(id));

            return NoContent();
        }

        private static string GetFeedbackKey(int id) => $"feedback_{id}";
    }
}
