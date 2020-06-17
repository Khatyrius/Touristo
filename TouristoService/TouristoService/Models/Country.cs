using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoService.Models
{
    public class Country
    {
        [Key] public int id { get; set; }
        [Required] public string name { get; set; }
        public ICollection<City> cities { get; set; }
    }
}
