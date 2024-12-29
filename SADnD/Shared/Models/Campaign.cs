using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SADnD.Shared.Models
{
    public class Campaign
    {
        public string Id { get; set; } = GenerateId();
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 5, ErrorMessage = "Name muss zwischen 5 und 50 Zeichen lang sein")]
        public string Name { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser>? EFDungeonMasters { get; set; }
        [JsonIgnore]
        public ICollection<ApplicationUser>? EFPlayers { get; set; }
        [NotMapped]
        public ICollection<User>? DungeonMasters { get; set; }
        [NotMapped]
        public ICollection<User>? Players { get; set; }
        public ICollection<JoinRequest>? JoinRequests { get; set; }
        public ICollection<Note>? Notes { get; set; }
        public ICollection<Character>? Characters { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
        private static string GenerateId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void RegenerateId()
        {
            Id = GenerateId();
        }
    }
}