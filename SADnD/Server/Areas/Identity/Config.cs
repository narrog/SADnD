using Duende.IdentityServer.Models;

namespace SADnD.Server.Areas.Identity
{
    public static class Config
    {
        // Definiere die IdentityResources
        public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),  // Standard OpenID Claims
            new IdentityResources.Profile(),   // Standard Profil-Claims (z.B. name, email)
            new IdentityResource(
                "campaign",                     // benutzerdefinierte IdentityResource
                new[] { "CampaignRole" }        // eigene Claims hinzufügen
            )
        };

        // Definiere die ApiScopes
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
            new ApiScope("api", "API Access", new[] { "CampaignRole" }) // API-Scope mit deinen Claims
            };

        // Definiere die Clients, die auf den IdentityServer zugreifen dürfen
        //public static IEnumerable<Client> Clients =>
        //    new List<Client>
        //    {
        //    new Client
        //    {
        //        ClientId = "your_client_id",
        //        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword, // Beispiel: Password-Grant
        //        ClientSecrets = { new Secret("your_secret".Sha256()) },
        //        AllowedScopes = { "openid", "profile", "api", "campaign" } // Zugelassene Scopes
        //    }
        //    };
    }
}
