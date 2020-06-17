using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DTO
{
    public class CountryDTO
    {
        public int id { get; set; }
        [Required] public string name { get; set; }
        public ICollection<City> cities { get; set; }
    }
}
