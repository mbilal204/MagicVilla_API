﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaDto
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Name { get; set; }
        public string Detail { get; set; }
        [Required]
        public double Rate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}