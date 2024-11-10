using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }
        public string CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
    }
}
