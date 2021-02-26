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
    [Route("api/[controller]")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("ax")]
        public async Task<IActionResult> Ax()
        {
            await Task.Delay(500);
            return Ok("ax");
        }

        [Authorize(Roles = Roles.User)]
        [HttpGet("bx")]
        public async Task<IActionResult> Bx()
        {
            await Task.Delay(500);
            return Ok("bx");
        }

        [Authorize(Roles = Roles.AllUsers)]
        [HttpGet("cx")]
        public async Task<IActionResult> Cx()
        {
            await Task.Delay(500);
            return Ok("cx");
        }

        [HttpGet("mov")]
        public async Task<IActionResult> Mov()
        {
            await Task.Delay(500);
            return Ok("mov");
        }
    }
}
