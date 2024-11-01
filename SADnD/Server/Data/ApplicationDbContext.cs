using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SADnD.Shared.Models;

namespace SADnD.Server.Data
{
    // Change the Path in the Package Manager Console to ./SADnD/Server to Add Migrations
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<JoinRequest> JoinRequests { get; set; }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<Class> Classes { get; set; }

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

            builder.Entity<Race>().HasData(
                new Race() { Name = "Zwerg" },
                new Race() { Name = "Elf" },
                new Race() { Name = "Halbling" },
                new Race() { Name = "Mensch" },
                new Race() { Name = "Aasimar" },
                new Race() { Name = "Drachenblütiger" },
                new Race() { Name = "Gnom" },
                new Race() { Name = "Goliath" },
                new Race() { Name = "Ork" },
                new Race() { Name = "Tiefling" }
            );
            builder.Entity<Class>().HasData(
                new Class() { Name = "Barbar" },
                new Class() { Name = "Barde" },
                new Class() { Name = "Druide" },
                new Class() { Name = "Hexenmeister" },
                new Class() { Name = "Kämpfer" },
                new Class() { Name = "Kleriker" },
                new Class() { Name = "Magier" },
                new Class() { Name = "Mönch" },
                new Class() { Name = "Paladin" },
                new Class() { Name = "Schurke" },
                new Class() { Name = "Waldläufer" },
                new Class() { Name = "Zauberer" }
            );
        }
    }
}
