using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CharacterSyncManager : IndexedDBSyncRepository<Character>
    {
        public CharacterSyncManager(IBlazorDbFactory dbFactory,CharacterManager characterManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB",true, dbFactory, characterManager,jsRuntime)
        {
        }
    }
}
