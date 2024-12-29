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
            var existingCharacter = await context.Characters
                .Include(c => c.EFUserAccess)
                .Include(c => c.Inventory)
                .Include(c => c.Classes)
                .FirstOrDefaultAsync(c => c.Id == entityToUpdate.Id);

            if (existingCharacter == null)
                throw new Exception("Character not found");

            // Synchronisiere Inventory
            foreach (var existingInventory in existingCharacter.Inventory.ToList())
            {
                var updatedInventory = entityToUpdate.Inventory.FirstOrDefault(i => i.Id == existingInventory.Id);
                if (updatedInventory != null)
                    context.Inventories.Entry(existingInventory).CurrentValues.SetValues(updatedInventory);
                else
                    context.Inventories.Remove(existingInventory);
            }
            foreach (var updatedInventory in entityToUpdate.Inventory)
            {
                if (!existingCharacter.Inventory.ToList().Any(i => i.Id == updatedInventory.Id))
                {
                    existingCharacter.Inventory.Add(updatedInventory);
                }
            }

            // Synchronisiere UserAccess
            var updatedUserAccess = entityToUpdate.EFUserAccess?.Select(user =>
                context.Users.Local.FirstOrDefault(u => u.Id == user.Id) ??
                context.Users.Attach(user).Entity
            ).ToList();
            updatedUserAccess = updatedUserAccess == null ? new List<ApplicationUser>() : updatedUserAccess;
            foreach (var existingUser in existingCharacter.EFUserAccess.ToList())
            {
                if (!updatedUserAccess.Any(u => u.Id == existingUser.Id))
                {
                    existingCharacter.EFUserAccess.Remove(existingUser);
                }
            }
            foreach (var updatedUser in updatedUserAccess)
            {
                if (!existingCharacter.EFUserAccess.Any(u => u.Id == updatedUser.Id))
                {
                    existingCharacter.EFUserAccess.Add(updatedUser);
                }
            }

            // Synchronisiere Classes
            foreach (var existingClass in existingCharacter.Classes.ToList())
            {
                var updatedClass = entityToUpdate.Classes.FirstOrDefault(i => i.Id == existingClass.Id);
                if (updatedClass != null)
                    context.Entry(existingClass).CurrentValues.SetValues(updatedClass);
                else
                    context.Remove(existingClass);
            }
            foreach (var updatedClass in entityToUpdate.Classes)
            {
                if (!existingCharacter.Classes.ToList().Any(i => i.Id == updatedClass.Id))
                {
                    existingCharacter.Classes.Add(updatedClass);
                }
            }

            //SynchronizeNavigationCollection(existingCharacter.UserAccess?? [], entityToUpdate.UserAccess);
            //SynchronizeNavigationCollection(existingCharacter.Inventory?? [], entityToUpdate.Inventory);
            //SynchronizeNavigationCollection(existingCharacter.Classes?? [], entityToUpdate.Classes);

            context.Entry(existingCharacter).CurrentValues.SetValues(entityToUpdate);

            await context.SaveChangesAsync();
            return entityToUpdate;
        }
        private void SynchronizeNavigationCollection<T>(ICollection<T> existingItems,ICollection<T>? updatedItems, Func<T,T,bool>? comparer = null)
        {
            if (updatedItems == null)
            {
                existingItems.Clear();
                return;
            }
            comparer ??= (a, b) => 
                a is { } && b is { }
                && context.Entry(a).Property("Id")?.CurrentValue?.Equals(context.Entry(b).Property("Id")?.CurrentValue) == true;
            
            foreach (var item in existingItems.ToList())
            {
                var updateItem = updatedItems.FirstOrDefault(x => comparer(x, item));
                if (updateItem != null)
                {
                    context.Entry(item).CurrentValues.SetValues(updateItem);
                }
                else
                    existingItems.Remove(item);
            }
            foreach (var item in updatedItems.ToList())
            {
                if (!existingItems.Any(x => comparer(x,item)))
                    existingItems.Add(item);
            }
        }
    }
}
