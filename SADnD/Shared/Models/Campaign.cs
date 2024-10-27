using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Campaign
    {
        public string Id { get; set; } = GenerateId();
        public string Name { get; set; }
        public ICollection<ApplicationUser> DungeonMasters { get; set; }
        public ICollection<ApplicationUser> Players { get; set; }
        public ICollection<JoinRequest> JoinRequests { get; set; }
        private static string GenerateId(int length = 8)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars,length)
                .Select(s => s[random.Next(s.Length)]).ToArray() );
        }
    }
}
