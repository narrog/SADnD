using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SADnD.Shared.Models;

namespace SADnD.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Campaign>()
                .HasIndex(c => c.Id)
                .IsUnique();

            builder.Entity<Campaign>()
                .HasMany(c => c.DungeonMasters)
                .WithMany(u => u.DungeonMasterCampaigns)
                .UsingEntity(j => j.ToTable("CampaignDungeonMasters"));

            builder.Entity<Campaign>()
                .HasMany(c => c.Players)
                .WithMany(u => u.PlayerCampaigns)
                .UsingEntity(j => j.ToTable("CampaignPlayers"));
        }
    }
}
