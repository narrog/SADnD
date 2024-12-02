using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class AppointmentApiManager : APIRepository<Appointment>
    {
        HttpClient _httpClient;
        public AppointmentApiManager(HttpClient httpClient)
            : base(httpClient, "appointment")
        {
            _httpClient = httpClient;
        }
    }
}
