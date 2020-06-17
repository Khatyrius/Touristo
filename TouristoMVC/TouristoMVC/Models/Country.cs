using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoMVC.Models
{
    public class Country
    {
        public int id { get; set; }

        [DisplayName("Name")]
        public string name { get; set; }

        [DisplayName("Cities")]
        public ICollection<City> cities { get; set; }
    }
}
