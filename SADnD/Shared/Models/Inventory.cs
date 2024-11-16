using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SADnD.Shared.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        [Range(1, int.MaxValue, ErrorMessage ="Anzahl muss mindestens 1 betragen")]
        public int Count { get; set; } = 1;
        public int CharacterId { get; set; }
        [JsonIgnore]
        public Character? Character { get; set; } 
        public int InventoryItemId { get; set; }
        public InventoryItem? Item { get; set; }
    }
}
