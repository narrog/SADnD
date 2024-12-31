using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? AppUser { get; set; }
        [NotMapped]
        public User? User { get; set; }
    }
}
