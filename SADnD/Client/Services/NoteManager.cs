using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class NoteManager : DataManager<Note>
    {
        public NoteManager(NoteApiManager apiManager)
            : base(apiManager)
        { 
        }
    }
}
