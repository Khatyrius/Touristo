using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DTO
{
    public class AttractionDTO
    {
        public int id { get; set; }
        [Required] public string name { get; set; }
        [Required] public string address { get; set; }
        [Required] public AttractionTypes type { get; set; }
        [Required] public City city { get; set; }
    }
}
