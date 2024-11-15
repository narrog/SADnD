using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, MinimumLength = 1, ErrorMessage = "Name muss zwischen 1 und 50 Zeichen lang sein")]
        public string Name { get; set; }
        public float? Weight { get; set; }
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
    }
}
