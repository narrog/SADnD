using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class RaceApiManager : APIRepository<Race>
    {
        HttpClient _httpClient;
        public RaceApiManager(HttpClient httpClient)
            : base(httpClient, "race")
        {
            _httpClient = httpClient;
        }
    }
}
