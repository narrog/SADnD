using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class ClassSyncManager : IndexedDBSyncRepository<Class>
    {
        public ClassSyncManager(IBlazorDbFactory dbFactory,ClassApiManager classManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, classManager,jsRuntime)
        {
        }
    }
}
