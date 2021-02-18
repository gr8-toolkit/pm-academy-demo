using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Contrllrs.Notes.Services;
using Contrllrs.Notes.Models;
using Microsoft.Extensions.Logging;

namespace Contrllrs.Notes.Controllers
{
    [ApiController]
    [Route("api/v1/user")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class UserController : ControllerBase
    {
        private readonly IStorage<User> _users;
        private readonly ILogger<UserController> _logger;

        public UserController(IStorage<User> users, ILogger<UserController> logger)
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
        public ActionResult<IEnumerable<ItemWithId<User>>> Get(/*default binding from query params*/ int? age)
        {
            var users = _users.GetAll();
            
            // filter via age parameter
            if (age.HasValue) users = users.Where(u => u.Item?.Age == age);
            
            return users.ToArray();
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult<User> GetById(int id)
        {
            var user = _users.Get(id);
            // return 404
            if (user == null) return NotFound();
            // return 200
            return user;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Dictionary<string, object>), (int)HttpStatusCode.BadRequest)]
        public IActionResult Post([FromBody]User user)
        {
            // option SuppressModelStateInvalidFilter should be enabled
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            _logger.LogInformation($"New user registration from {Platform}");

            var id = _users.Add(user);

            // return 201
            return CreatedAtAction(nameof(GetById), new {id}, id);
        }
    }
}
