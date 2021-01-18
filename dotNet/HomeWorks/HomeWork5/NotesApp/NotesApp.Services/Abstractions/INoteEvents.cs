using System;
using NotesApp.Services.Models;

namespace NotesApp.Services.Abstractions
{
    public interface INoteEvents
    {
        void NotifyAdded(Note note, int userId);
        void NotifyDeleted(Guid noteId, int userId);
    }
}
