using SADnD.Shared.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

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

        public async Task<IEnumerable<Campaign>?> GetAllCampaignsAsync() {
            try {
                var result = await _httpClient.GetAsync("campaign");
                result.EnsureSuccessStatusCode();
                
                var apiResponse = await result.Content.ReadFromJsonAsync<APIListOfEntityResponse<Campaign>>();
                if (apiResponse.Success)
                    return apiResponse.Data;
                else
                    return new List<Campaign>();
            }
            catch (Exception ex) {
                var msg = ex.Message;
                return null;
            }
            //    var response = await _httpClient.GetAsync("campaign");

            //if (response.IsSuccessStatusCode) {
            //    var apiResponse = await response.Content.ReadFromJsonAsync<APIListOfEntityResponse<Campaign>>();
            //    return (IEnumerable<Campaign>)(apiResponse?.Data ?? new IEnumerable<Campaign>());
            //}
            //else {
            //    Console.WriteLine("Fehler beim Abrufen der Kampagnen (CampaignManager)");
            //}
            //return null;
        }
    }
}
