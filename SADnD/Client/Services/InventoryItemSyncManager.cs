using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class InventoryItemSyncManager : IndexedDBSyncRepository<InventoryItem>
    {
        public InventoryItemSyncManager(IBlazorDbFactory dbFactory,InventoryItemApiManager itemManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, itemManager,jsRuntime)
        {
        }
    }
}
