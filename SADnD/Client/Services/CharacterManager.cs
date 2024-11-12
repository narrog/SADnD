using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CharacterManager : APIRepository<Character>
    {
        HttpClient _httpClient;
        public CharacterManager(HttpClient httpClient)
            : base(httpClient, "character")
        {
            _httpClient = httpClient;
        }
    }
}
