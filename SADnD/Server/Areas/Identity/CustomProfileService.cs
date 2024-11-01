using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using SADnD.Shared.Models;

namespace SADnD.Server.Areas.Identity
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var user = await _userManager.GetUserAsync(context.Subject);
            var userClaims = await _userManager.GetClaimsAsync(user);

            context.IssuedClaims.AddRange(userClaims);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            // context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
