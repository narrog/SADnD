using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SADnD.Shared.Models
{
    public abstract class Note
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string? CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
        public int? CharacterId { get; set; }
        [JsonIgnore]
        public Character? Character { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        [JsonIgnore]
        public ICollection<Note>? Notes { get; set; }
        [JsonIgnore]
        public ICollection<Note>? NoteMentions { get; set; }
    }
}
