using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SADnD.Server.Data;
using SADnD.Shared.Models;
using System.Security.Claims;

namespace SADnD.Server.Areas.Identity
{
    public class CustomClaimsService<TDataContext,UserManager>
        where TDataContext : DbContext
        where UserManager : UserManager<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public CustomClaimsService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // TODO: Optimise with granular add and remove functions or move entirely to onLogin maybe
        public async Task AddCampaignClaims(ApplicationUser user)
        {
            var userWithCampaigns = await _context.Users
                .Include(u => u.DungeonMasterCampaigns)
                .Include(u => u.PlayerCampaigns)
                .FirstOrDefaultAsync();
            if (userWithCampaigns != null)
            {
                await _userManager.RemoveClaimsAsync(user,await _userManager.GetClaimsAsync(user));
                var claims = new List<Claim>();
                foreach (var master in userWithCampaigns.DungeonMasterCampaigns.ToList())
                {
                    claims.Add(new Claim("CampaignRole", $"{master.Id}:DungeonMaster"));
                    claims.Add(new Claim("Campaign", master.Id));
                }
                foreach (var player in userWithCampaigns.PlayerCampaigns.ToList())
                {
                    claims.Add(new Claim("CampaignRole", $"{player.Id}:Player"));
                    claims.Add(new Claim("Campaign", player.Id));
                }
                var identity = new ClaimsIdentity(claims);
                await _userManager.AddClaimsAsync(user, claims);
            }
        }
    }
}
