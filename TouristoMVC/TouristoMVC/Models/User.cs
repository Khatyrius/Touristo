using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoMVC.Models
{
    public class User
    {
        [DisplayName("ID")]
        public int id { get; set; }

        [DisplayName("Username")]
        public string username { get; set; }

        public string password { get; set; }

        [DisplayName("Role")]
        public string role { get; set; }
    }
}
