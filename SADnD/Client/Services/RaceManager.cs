using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class RaceManager : APIRepositoryGeneric<Race>
    {
        HttpClient _httpClient;
        public RaceManager(HttpClient httpClient)
            : base(httpClient, "race")
        {
            _httpClient = httpClient;
        }
    }
}
