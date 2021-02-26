using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Authorization;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = Roles.AllUsers)]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">item id.</param>
        /// <returns>item entry</returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var todo = new TodoItem();
            return Ok(todo);
        }

        [HttpGet]
        [Route("")]
        [Route("get-all")]
        public async Task<IActionResult> GetAllItems()
        {
            var todo = new TodoItem();
            return Ok(todo);
        }

        [HttpPost]
        [Route("insert")]
        public async Task<IActionResult> AddItem(TodoItem item)
        {
            return Ok();
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateItem(int id, [FromBody] TodoItem item)
        {
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteItemById(int id)
        {
            return NoContent();
        }

    }
}
