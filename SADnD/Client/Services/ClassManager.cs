using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class ClassManager : APIRepositoryGeneric<Class>
    {
        HttpClient _httpClient;
        public ClassManager(HttpClient httpClient)
            : base(httpClient, "class")
        {
            _httpClient = httpClient;
        }
    }
}
