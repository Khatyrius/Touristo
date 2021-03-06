﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoService.Models
{
    public class Attraction
    {
        [Key] public int id { get; set; }
        [Required] public string name { get; set; }
        [Required] public string address { get; set; }
        [Required] public AttractionTypes type { get; set; }
        [Required] public City city { get; set; }
    }
}
