using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADnD.Shared.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "Name muss zwischen 1 und 50 Zeichen lang sein")]
        public string Name { get; set; }
        [Range (0, int.MaxValue, ErrorMessage = "Gewicht darf nicht negativ sein")]
        public float? Weight { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? AppUser { get; set; }
        [NotMapped]
        public User? User { get; set; }
    }
}
