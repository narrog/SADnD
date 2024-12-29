using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADnD.Shared.Models
{
    public class AppointmentVote
    {
        public int Id { get; set; }
        [ForeignKey("AppUser")]
        public string UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? AppUser { get; set; }
        [NotMapped]
        public User? User { get; set; }
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public Appointment? Appointment { get; set; }
        public string Reaction { get; set; }
        public string? Comment { get; set; }
    }
}
