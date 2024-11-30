using Microsoft.AspNetCore.Components;

namespace SADnD.Client.Shared
{
    public abstract class NotesCategoryParent : ComponentBase
    {
        public virtual string ActiveCategory { get; set; } = "";
        public bool ShowAddNotes { get; private set; } = false;
        public int SelectedNoteId { get; private set; } = 0;
        public void HandleCategoryChanged()
        {
            ShowAddNotes = false;
        }
        public Task ShowAddNotesAsync(int noteId)
        {
            SelectedNoteId = noteId;
            ShowAddNotes = true;
            return Task.CompletedTask;
        }
        public Task HideAddNotesAsync()
        {
            ShowAddNotes = false;
            return Task.CompletedTask;
        }
    }
}
