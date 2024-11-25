using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class JoinRequestManager : APIRepository<JoinRequest>
    {
        HttpClient _httpClient;
        public JoinRequestManager(HttpClient httpClient)
            : base(httpClient, "joinRequest")
        {
            _httpClient = httpClient;
        }
    }
}
