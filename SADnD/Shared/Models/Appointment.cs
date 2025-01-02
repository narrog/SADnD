using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SADnD.Shared.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateOnly Date {  get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool Accepted { get; set; }
        public int CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
        public ICollection<AppointmentVote>? AppointmentVotes { get; set; }

        [NotMapped] 
        public string StartTimeInput { get; set; } = string.Empty;

        [NotMapped]
        public string EndTimeInput { get; set; } = string.Empty;
    }
}
