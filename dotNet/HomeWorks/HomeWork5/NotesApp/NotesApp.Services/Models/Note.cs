using System;

namespace NotesApp.Services.Models
{
    public class Note
    {
        public Guid Id { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }
    }
}
