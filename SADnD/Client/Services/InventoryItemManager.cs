using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class InventoryItemManager : APIRepositoryGeneric<InventoryItem>
    {
        HttpClient _httpClient;
        public InventoryItemManager(HttpClient httpClient)
            : base(httpClient, "inventoryItem")
        {
            _httpClient = httpClient;
        }
    }
}
