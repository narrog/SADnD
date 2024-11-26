﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Campaign
    {
        public string Id { get; set; } = GenerateId();
        [Required]
        [StringLength(maximumLength:50, MinimumLength = 5, ErrorMessage ="Name muss zwischen 5 und 50 Zeichen lang sein")]
        public string Name { get; set; }
        public ICollection<ApplicationUser>? DungeonMasters { get; set; }
        public ICollection<ApplicationUser>? Players { get; set; }
        public ICollection<JoinRequest>? JoinRequests { get; set; }
        public ICollection<Note>? Notes { get; set; }
        public ICollection<Character>? Characters { get; set; }
        private static string GenerateId(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars,length)
                .Select(s => s[random.Next(s.Length)]).ToArray() );
        }
        public void RegenerateId()
        {
            Id = GenerateId();
        }
    }
}
