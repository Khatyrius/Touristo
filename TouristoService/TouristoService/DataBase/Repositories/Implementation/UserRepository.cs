using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.DataBase.Repositories.Interfaces;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(User tEntity)
        {
            if (!CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                await _context.AddAsync(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public bool CheckIfExists(int id)
        {
            return _context.Users.Any(u => u.id == id);
        }

        public bool CheckIfExists(User entity)
        {
            var serachByUsername = from user in _context.Users
                                   where user.username.Equals(entity.username, StringComparison.InvariantCultureIgnoreCase)
                                   select user;

            if (serachByUsername.Any())
            {
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            if (CheckIfExists(id))
            {
                _context.Users.Remove(_context.Users.Single(u => u.id == id));
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Delete(User tEntity)
        {
            if (CheckIfExists(tEntity))
            {
                _context.Users.Remove(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<User>> GetAll()
        {
             return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.id == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.username == username);
        }

        public int GetLast()
        {
            var users = _context.Users.ToListAsync();
            int id = users.Result.Select(x => x.id).Max();
            return id;
        }

        public async Task<bool> Update(User tEntity)
        {
            if (CheckIfExists(tEntity.id))
            {
                _context.Users.Update(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Validate(string username, string password)
        {
            var query = from user in _context.Users
                        where user.username == username &&
                              user.password == password
                        select user;

            if (await query.AnyAsync())
            {
                return true;
            }

            return false;
        }
    }
}
