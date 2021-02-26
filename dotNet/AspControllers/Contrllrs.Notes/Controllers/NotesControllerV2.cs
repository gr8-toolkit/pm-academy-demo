using Microsoft.AspNetCore.Mvc;
using System;
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
    [Route("api/v2/note")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotesControllerV2 : ControllerBase
    {
        private readonly IStorage<Note> _notes;

        public NotesControllerV2(IStorage<Note> notes)
        {
            _notes = notes;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<ItemWithId<Note>>> Get()
        {
            // C# doesn't support implicit cast operators on interfaces.
            // Need to use .ToArray().
            return _notes.GetAll().ToArray();
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public ActionResult<Note> GetById(int id)
        {
            var note = _notes.Get(id);

            // return 404
            if (note == null) return NotFound();
            
            // return 200
            return note;
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public IActionResult Post(
            string noteText, 
            [FromHeader(Name = "X-Platform")] string platform,
            [FromServices]ILoggerFactory loggerFactory)
        {
            noteText = noteText?.Trim();
            if (string.IsNullOrEmpty(noteText))
            {
                // return 400
                return BadRequest("Can't create empty note");
            }
            
            var note = new Note
            {
                Header = noteText.Substring(0, Math.Min(noteText.Length, 32)).Trim(),
                Text = noteText
            };
            var id = _notes.Add(note);

            var logger = loggerFactory.CreateLogger<NotesControllerV2>();
            logger.LogInformation($"Note {id} was created from {platform}");

            // return 201
            return CreatedAtAction(nameof(GetById), new {id}, id);
        }
    }
}
