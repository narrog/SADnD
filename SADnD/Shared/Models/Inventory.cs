using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public int Count { get; set; } = 1;
        public int CharacterId { get; set; }
        public Character? Character { get; set; }
        public int InventoryItemId { get; set; }
        public InventoryItem? Item { get; set; }
    }
}
