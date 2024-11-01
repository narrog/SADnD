using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SADnD.Shared.Models;
using System.Security.Claims;

namespace SADnD.Server.Areas.Identity
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var userClaims = await _userManager.GetClaimsAsync(user);
            identity.AddClaims(userClaims);

            return identity;
        }
    }
}
