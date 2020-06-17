using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> Validate(string username, string password);
        Task<User> GetByUsername(string username);
    }
}
