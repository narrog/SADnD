using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class ClassApiManager : APIRepository<Class>
    {
        HttpClient _httpClient;
        public ClassApiManager(HttpClient httpClient)
            : base(httpClient, "class")
        {
            _httpClient = httpClient;
        }
    }
}
