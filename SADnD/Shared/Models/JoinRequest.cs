using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class JoinRequest
    {
        public int Id { get; set; }
        public DateTime? Accepted { get; set; }
        public string CampaignId { get; set; }
        public Campaign Campaign { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
