using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float? Weight { get; set; }
        public int CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
    }
}
