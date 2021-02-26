using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Contrllrs.Notes.Services;
using Contrllrs.Notes.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Contrllrs.Notes.Controllers
{
    [ApiController]
    [Route("api/v2/user")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserControllerV2 : ControllerBase
    {
        private readonly IStorage<User> _users;
        private readonly ILogger<UserControllerV2> _logger;

        public UserControllerV2(IStorage<User> users, ILogger<UserControllerV2> logger)
        {
            _users = users;
            _logger = logger;
        }

        [FromHeader(Name = "X-Platform")]
        public string Platform { get; set; }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="age">Optional age filter.</param>
        /// <returns>Returns users list.</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ItemWithId<User>>>> GetAsync(/*default binding from query params*/ int? age)
        {
            var users = await _users.GetAllAsync();
            
            // filter via age parameter
            if (age.HasValue) users = users.Where(u => u.Item?.Age == age);
            
            return users.ToArray();
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<User>> GetByIdAsync(int id)
        {
            var user = await _users.GetAsync(id);
            // return 404
            if (user == null) return NotFound();
            // return 200
            return user;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Dictionary<string, object>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody]User user)
        {
            // option SuppressModelStateInvalidFilter should be enabled
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            _logger.LogInformation($"New user registration from {Platform}");

            var id = await _users.AddAsync(user);

            // return 201
            return CreatedAtAction(nameof(GetByIdAsync), new {id}, id);
        }
    }
}
