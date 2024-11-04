﻿using Newtonsoft.Json;
using System;
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
        public int TemporaryHitpoints { get; set; } = 0;
        public bool[]? DeathRoles { get; set; }
        public string? Background {  get; set; }
        [Range (1, int.MaxValue, ErrorMessage = "Alter ungültig")]
        public int? Age { get; set; }
        // Auswahl: S, M, L
        public char? SizeCategory { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Grösse ungültig")]
        public float? Size {  get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Gewicht ungültig")]
        public float? Weight { get; set; }
        // maybe dropdown?
        public string? Sex { get; set; }
        // Auswahl: Rechtschaffen Gut, Neutral Gut, Chaotisch Gut, Rechtschaffen Neutral, Neutral, Chaotisch Neutral, Rechtschaffen Böse, Neutral Böse, Chaotisch Böse
        public string? Alignment { get; set; }
        public string? UserId { get; set; }
        [JsonIgnore]
        public ApplicationUser? User { get; set; }
        public string? CampaignId { get; set; }
        [JsonIgnore]
        public Campaign? Campaign { get; set; }
    }
}