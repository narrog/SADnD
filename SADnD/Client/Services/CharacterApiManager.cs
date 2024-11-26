using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CharacterApiManager : APIRepository<Character>
    {
        HttpClient _httpClient;
        public CharacterApiManager(HttpClient httpClient)
            : base(httpClient, "character")
        {
            _httpClient = httpClient;
        }
    }
}
