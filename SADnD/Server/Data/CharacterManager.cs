using IdentityModel;
using Microsoft.EntityFrameworkCore;
using SADnD.Shared.Models;

namespace SADnD.Server.Data
{
    public class CharacterManager : EFRepositoryGeneric<Character,ApplicationDbContext>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Character> _characters;

        public CharacterManager(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
            _characters = context.Characters;
        }
        public override async Task<Character> Update(Character entityToUpdate)
        {
            var inventories = context.Inventories.Where(i => i.CharacterId == entityToUpdate.Id).AsNoTracking().ToList();
            foreach (var item in inventories)
            {
                if (!entityToUpdate.Inventory.Any(i => i.Id == item.Id))
                    context.Inventories.Remove(item);
            }
            dbSet.Update(entityToUpdate);
            await context.SaveChangesAsync();
            return entityToUpdate;
        }
    }
}
