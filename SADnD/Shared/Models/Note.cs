using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SADnD.Shared.Models
{
    public abstract class Note
    {
        public Note()
        {
            Type = GetType().Name;
        }
        public int Id { get; set; }
        [Required (ErrorMessage = "Titel ist ein Pflichtfeld")]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "Name muss zwischen 1 und 50 Zeichen lang sein")]
        public string Name { get; set; }
        [Required (ErrorMessage = "Text ist ein Pflichtfeld")]
        public string Content { get; set; }
        public string Type { get; set; }
        public int? CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
        public int? CharacterId { get; set; }
        [JsonIgnore]
        public Character? Character { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? AppUser { get; set; }
        [NotMapped]
        public User? User { get; set; }
        [JsonIgnore]
        public ICollection<Note>? Notes { get; set; }
        [JsonIgnore]
        public ICollection<Note>? NoteMentions { get; set; }
    }
}
