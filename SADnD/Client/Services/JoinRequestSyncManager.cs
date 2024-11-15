using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class JoinRequestSyncManager : IndexedDBSyncRepository<JoinRequest>
    {
        public JoinRequestSyncManager(IBlazorDbFactory dbFactory,JoinRequestManager requestManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB",true, dbFactory, requestManager,jsRuntime)
        {
        }
    }
}
