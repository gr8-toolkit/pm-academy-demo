using System;
using NotesApp.Services.Models;

namespace NotesApp.Services.Abstractions
{
    public interface INotesStorage
    {
        void AddNote(Note note, int userId);
        bool DeleteNote(Guid id);
    }
}
