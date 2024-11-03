﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SADnD.Shared.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Name darf max. 50 Zeichen lang sein")]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Rasse muss gewählt werden.")]
        public int RaceId { get; set; }
        public Race? Race { get; set; }
        public ICollection<CharacterClass>? Classes { get; set; }
        public int Hitpoints { get; set; }
        public int TemporaryHitpoints { get; set; }
        public bool[]? DeathRoles { get; set; }
        public string? Background {  get; set; }
        public int? Age { get; set; }
        public char? SizeCategory { get; set; }
        public float? Size {  get; set; }
        public float? Weight { get; set; }
        public string? Sex { get; set; }
        public string? Alignment { get; set; }
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public string? CampaignId { get; set; }
        public Campaign? Campaign { get; set; }
    }
}