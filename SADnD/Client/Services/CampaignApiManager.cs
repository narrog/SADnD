using SADnD.Client.Shared;
using SADnD.Shared.Models;
using System.Diagnostics;
using System.Net;

namespace SADnD.Client.Services
{
    public class CampaignApiManager : APIRepository<Campaign>
    {
        HttpClient _httpClient;
        public CampaignApiManager(HttpClient httpClient)
            : base(httpClient, "campaign")
        {
            _httpClient = httpClient;
        }
    }
}
