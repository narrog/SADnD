using SADnD.Shared.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

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
