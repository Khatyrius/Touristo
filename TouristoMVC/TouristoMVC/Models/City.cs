using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoMVC.Models
{
    public class City
    {
        [DisplayName("Id")]
        public int id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Country")]
        public Country country { get; set; }
        [DisplayName("Attractions")]
        public ICollection<Attraction> attractions { get; set; }
    }
}
