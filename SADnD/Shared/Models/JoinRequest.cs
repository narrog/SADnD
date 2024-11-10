using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class JoinRequest
    {
        public int Id { get; set; }
        public DateTime? Accepted { get; set; }
        [Required (ErrorMessage = "Zugangscode ist ein Pflichtfeld")]
        [StringLength(maximumLength: 8, MinimumLength = 8, ErrorMessage = "Länge von Zugangscode falsch")]
        public string CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
