using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class JoinRequestApiManager : APIRepository<JoinRequest>
    {
        HttpClient _httpClient;
        public JoinRequestApiManager(HttpClient httpClient)
            : base(httpClient, "joinRequest")
        {
            _httpClient = httpClient;
        }
    }
}
