using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class JoinRequestSyncManager : IndexedDBSyncRepository<JoinRequest>
    {
        public JoinRequestSyncManager(IBlazorDbFactory dbFactory,JoinRequestApiManager requestManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, requestManager,jsRuntime)
        {
        }
    }
}
