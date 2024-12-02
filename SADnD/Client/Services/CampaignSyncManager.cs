using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CampaignSyncManager : IndexedDBSyncRepository<Campaign>
    {
        public CampaignSyncManager(IBlazorDbFactory dbFactory,CampaignApiManager campaignManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, campaignManager,jsRuntime)
        {
        }
    }
}
