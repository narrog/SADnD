using SADnD.Client.Shared;
using SADnD.Shared.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

namespace SADnD.Client.Services
{
    public class CampaignManager : APIRepository<Campaign>
    {
        HttpClient _httpClient;
        public CampaignManager(HttpClient httpClient)
            : base(httpClient, "campaign")
        {
            _httpClient = httpClient;
        }
    }
}
