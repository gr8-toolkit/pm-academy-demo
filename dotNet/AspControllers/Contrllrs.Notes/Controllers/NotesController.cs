using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using Contrllrs.Notes.Services;
using Contrllrs.Notes.Models;

namespace Contrllrs.Notes.Controllers
{
    [ApiController]
    [Route("api/v1/note")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class NotesController : ControllerBase
    {
        private readonly IStorage<Note> _notes;

        public NotesController(IStorage<Note> notes)
        {
            _notes = notes;
        }

        [HttpGet]
        public IEnumerable<ItemWithId<Note>> Get()
        {
            return _notes.GetAll();
        }

        [HttpGet("{id}")]
        public Note Get(int id)
        {
            // - How to return NotFound (404) ?
            // - Need to use IActionResult !
            return _notes.Get(id);
        }

        [HttpPost]
        public int Post(string noteText)
        {
            // - How to return BadRequest (400) ?
            // - Need to use IActionResult !
            
            var note = new Note
            {
                Header = noteText?.Substring(0, Math.Min(noteText.Length, 32)).Trim(),
                Text = noteText
            };
            
            return _notes.Add(note);
        }
    }
}
