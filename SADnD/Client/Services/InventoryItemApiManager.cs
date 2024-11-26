using SADnD.Client.Shared;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class InventoryItemApiManager : APIRepository<InventoryItem>
    {
        HttpClient _httpClient;
        public InventoryItemApiManager(HttpClient httpClient)
            : base(httpClient, "inventoryItem")
        {
            _httpClient = httpClient;
        }
    }
}
