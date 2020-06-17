using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using TouristoMVC.Models;

namespace TouristoMVC.Models
{
    public class Attraction
    {
        [DisplayName("Id")]
        public int id { get; set; }
        [DisplayName("Name")]
        public string name { get; set; }
        [DisplayName("Address")]
        public string address { get; set; }
        [DisplayName("Type")]
        public AttractionTypes type { get; set; }
        [DisplayName("City")]
        public City city { get; set; }
    }
}
