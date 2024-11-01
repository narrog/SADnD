using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RaceId { get; set; }
        public Race Race { get; set; }
        public ICollection<Models.Class> Classes { get; set; }
        public int Hitpoints { get; set; }
        public int TemporaryHitpoints { get; set; }
        public bool[] DeathRoles { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string? CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
    }
}
