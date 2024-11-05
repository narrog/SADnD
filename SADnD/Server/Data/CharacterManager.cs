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
    }
}
