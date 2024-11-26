using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CharacterSyncManager : IndexedDBSyncRepository<Character>
    {
        public CharacterSyncManager(IBlazorDbFactory dbFactory,CharacterApiManager characterManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, characterManager,jsRuntime)
        {
        }
    }
}
