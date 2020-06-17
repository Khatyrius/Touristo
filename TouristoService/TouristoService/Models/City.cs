using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoService.Models
{
    public class City
    {
        [Key] public int id { get; set; }
        [Required] public string name { get; set; }
        [Required] public Country country { get; set; }
        public ICollection<Attraction> attractions { get; set; }
    }
}
