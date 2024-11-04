using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace SADnD.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        [JsonIgnore]
        public ICollection<Campaign> DungeonMasterCampaigns { get; set; }
        [JsonIgnore]
        public ICollection<Campaign> PlayerCampaigns { get; set; }
        [JsonIgnore]
        public ICollection<JoinRequest> JoinRequests { get; set; }
    }
}
