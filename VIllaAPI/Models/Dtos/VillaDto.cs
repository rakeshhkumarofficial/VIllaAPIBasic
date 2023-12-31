﻿using System.ComponentModel.DataAnnotations;

namespace VIllaAPI.Models.Dtos
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
    }
}
