﻿using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class RaceSyncManager : IndexedDBSyncRepository<Race>
    {
        public RaceSyncManager(IBlazorDbFactory dbFactory,RaceManager raceManager,IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB",true, dbFactory, raceManager,jsRuntime)
        {
        }
    }
}
