using BlazorDB;
using SADnD.Shared.Models;

namespace SADnD.Client.Services
{
    public class CharacterIndexedDBRepository : IndexedDBRepository<Character>
    {
        public CharacterIndexedDBRepository(IBlazorDbFactory dbFactory)
            : base("SADnD.IndexedDB",true, dbFactory)
        {
        }
    }
}
