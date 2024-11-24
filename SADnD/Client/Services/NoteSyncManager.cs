using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class NoteSyncManager : IndexedDBSyncRepository<Note>
    {
        public NoteSyncManager(IBlazorDbFactory dbFactory,NoteManager noteManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, noteManager, jsRuntime)
        {
        }
    }
}
