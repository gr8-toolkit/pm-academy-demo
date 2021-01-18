using System;
using NotesApp.Services.Abstractions;
using NotesApp.Services.Models;

namespace NotesApp.Services.Services
{
    public class NotesService: INotesService
    {
        private readonly INotesStorage _storage;
        private readonly INoteEvents _events;

        public NotesService(INotesStorage storage, INoteEvents events)
        {
            _storage = storage;
            _events = events;
        }
        public void AddNote(Note note, int userId)
        {
            if (note == null) throw new ArgumentNullException(nameof(note));
            _storage.AddNote(note, userId);
            _events.NotifyAdded(note, userId);
        }

        public bool DeleteNote(Guid noteId, int userId)
        {
            var deleted = _storage.DeleteNote(noteId);
            if (deleted) _events.NotifyDeleted(noteId, userId);
            return deleted;
        }
    }
}
