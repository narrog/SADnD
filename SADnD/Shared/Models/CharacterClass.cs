using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class CharacterClass
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}
