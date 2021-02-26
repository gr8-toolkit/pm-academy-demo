using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Handlers;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomContactsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RandomContactsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get batch of contacts.
        /// </summary>
        /// <param name="skip">number of skipped items.</param>
        /// <param name="limit">number of requested items.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetContacts(int skip = 0, int limit = 100)
        {
            var request = new GetContactsRequest
            {
                Limit = limit,
                Skip = skip
            };

            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}
