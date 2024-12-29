using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Name darf max. 50 Zeichen lang sein")]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Rasse muss gewählt werden.")]
        public int RaceId { get; set; }
        public Race? Race { get; set; }
        public ICollection<CharacterClass>? Classes { get; set; }
        public int? ArmorClass {  get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "HP ungültig")]
        public int MaxHitpoints { get; set; }
        public int Hitpoints { get; set; }
        public int TemporaryHitpoints { get; set; } = 0;
        public List<bool>? DeathRoles { get; set; }
        public string? Background {  get; set; }
        [Range (1, int.MaxValue, ErrorMessage = "Alter ungültig")]
        public int? Age { get; set; }
        public char? SizeCategory { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Grösse ungültig")]
        public float? Size {  get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Gewicht ungültig")]
        public float? Weight { get; set; }
        public string? Sex { get; set; }
        public string? Alignment { get; set; }
        [ForeignKey("AppUser")]
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? AppUser { get; set; }
        [NotMapped]
        public User? User { get; set; }
        public string? CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
        public ICollection<Inventory>? Inventory { get; set; }
        public ICollection<Note>? Notes { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser>? EFUserAccess { get; set; }
        [NotMapped]
        public ICollection<User>? UserAccess { get; set; }
    }
}
