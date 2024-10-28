using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CampaignManager : APIRepositoryGeneric<Campaign>
    {
        HttpClient _httpClient;
        public CampaignManager(HttpClient httpClient)
            : base(httpClient, "campaign")
        {
            _httpClient = httpClient;
        }
    }
}
