using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class AppointmentVote
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser? User {  get; set; }
        public int AppointmentId { get; set; }
        [JsonIgnore]
        public Appointment? Appointment { get; set; }
        // Ja/Nein/Vielleicht
        public string Reaction { get; set; }
        public string? Comment { get; set; }
    }
}
