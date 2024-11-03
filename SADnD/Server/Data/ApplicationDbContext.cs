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
        public DbSet<CharacterClass> CharactersClasses { get; set; }

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
                new Race() { Id = 1, Name = "Zwerg" },
                new Race() { Id = 2, Name = "Elf" },
                new Race() { Id = 3, Name = "Halbling" },
                new Race() { Id = 4, Name = "Mensch" },
                new Race() { Id = 5, Name = "Aasimar" },
                new Race() { Id = 6, Name = "Drachenblütiger" },
                new Race() { Id = 7, Name = "Gnom" },
                new Race() { Id = 8, Name = "Goliath" },
                new Race() { Id = 9, Name = "Ork" },
                new Race() { Id = 10, Name = "Tiefling" }
            );
            builder.Entity<Class>().HasData(
                new Class() { Id = 1, Name = "Barbar" },
                new Class() { Id = 2, Name = "Barde" },
                new Class() { Id = 3, Name = "Druide" },
                new Class() { Id = 4, Name = "Hexenmeister" },
                new Class() { Id = 5, Name = "Kämpfer" },
                new Class() { Id = 6, Name = "Kleriker" },
                new Class() { Id = 7, Name = "Magier" },
                new Class() { Id = 8, Name = "Mönch" },
                new Class() { Id = 9, Name = "Paladin" },
                new Class() { Id = 10, Name = "Schurke" },
                new Class() { Id = 11, Name = "Waldläufer" },
                new Class() { Id = 12, Name = "Zauberer" }
            );
        }
    }
}
