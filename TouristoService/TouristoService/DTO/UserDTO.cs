using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoService.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        [Required] public string username { get; set; }
        [Required] public string password { get; set; }
        public string role { get; set; }
    }
}
