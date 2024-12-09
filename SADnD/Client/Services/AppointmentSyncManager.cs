using BlazorDB;
using Microsoft.JSInterop;
using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class AppointmentSyncManager : IndexedDBSyncRepository<Appointment>
    {
        public AppointmentSyncManager(IBlazorDbFactory dbFactory, AppointmentApiManager appointmentManager, IJSRuntime jsRuntime)
            : base("SADnD.IndexedDB", dbFactory, appointmentManager, jsRuntime)
        {
        }
    }
}
