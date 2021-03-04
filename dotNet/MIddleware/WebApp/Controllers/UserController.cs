using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Authorization;

namespace WebApp.Controllers
{
    [Authorize(AuthenticationSchemes = Consts.RapidUser, Roles = Roles.User)]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("info")]
        public async Task<IActionResult> GetUserInfo()
        {
            if (!(User.Identity is RapidUserIdentity))
            {
                return Unauthorized();
            }

            await Task.Delay(100);
            return Ok(User.Identity.Name);
        }
    }
}
