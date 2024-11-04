using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class CharacterClass
    {
        public int Id { get; set; }
        [Range (0, 20, ErrorMessage = "Maximum Level: 20")]
        public int Level { get; set; }
        public int CharacterId { get; set; }
        [JsonIgnore] 
        public Character? Character { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Klasse muss gewählt werden.")]
        public int ClassId { get; set; }
        public Class? Class { get; set; }
    }
}
