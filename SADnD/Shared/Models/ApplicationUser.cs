using Microsoft.AspNetCore.Identity;

namespace SADnD.Shared.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Campaign> DungeonMasterCampaigns { get; set; }
        public ICollection<Campaign> PlayerCampaigns { get; set; }
        public ICollection<JoinRequest> JoinRequests { get; set; }
    }
}
