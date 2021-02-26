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
    //[Authorize(AuthenticationSchemes = Consts.RapidService, Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("info")]
        public async Task<IActionResult> GetAdminInfo()
        {
            throw new Exception("Test");

            if (!(User.Identity is RapidServiceIdentity))
            {
                return Unauthorized();
            }

            await Task.Delay(100);
            return Ok(User.Identity.Name);
        }

        [HttpGet("info-all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetInfo()
        {
            return Ok("test");
        }
    }
}
