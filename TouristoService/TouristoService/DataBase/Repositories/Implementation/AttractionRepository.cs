using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.DataBase.Repositories.Interfaces;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Implementation
{
    public class AttractionRepository : IAttractionRepostiory
    {
        private readonly DataContext _context;

        public AttractionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Attraction tEntity)
        {
            tEntity.city = CheckIfCreateNewCity(tEntity.city);

            if (!CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                await _context.Attractions.AddAsync(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private City CheckIfCreateNewCity(City entity)
        {
            entity.country = CheckIfCreateNewCountry(entity.country);

            var query = from city in _context.Cities
                        where city.name.Equals(entity.name, StringComparison.InvariantCultureIgnoreCase)
                        && city.country.name.Equals(entity.country.name, StringComparison.InvariantCultureIgnoreCase)
                        select city;

            if (query.Any())
            {
                entity = query.First();
                return entity;
            }

            entity.id = 0;  
            return entity;
        }

        private Country CheckIfCreateNewCountry(Country entity)
        {
            var query = from c in _context.Countries
                        where c.name.Equals(entity.name, StringComparison.InvariantCultureIgnoreCase)
                        select c;

            if (query.Any())
            {
                entity = query.First();
                return entity;
            }

            entity.id = 0;
            return entity;
        }

        public bool CheckIfExists(int id)
        {
            return _context.Attractions.Any(e => e.id == id);
        }

        public bool CheckIfExists(Attraction entity)
        {
            var query = from attraction in _context.Attractions
                        where attraction.name.Equals(entity.name, StringComparison.InvariantCultureIgnoreCase)
                        && attraction.address.Equals(entity.address, StringComparison.InvariantCultureIgnoreCase)
                        && attraction.city.name.Equals(entity.city.name, StringComparison.InvariantCultureIgnoreCase)
                        && attraction.type.Equals(entity.type)
                        select attraction;

            return query.Any();
        }

        public async Task<bool> Delete(int id)
        {
            if (CheckIfExists(id))
            {
                _context.Attractions.Remove(_context.Attractions.Single(attraction => attraction.id == id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Attraction tEntity)
        {
            if (CheckIfExists(tEntity.id))
            {
                _context.Attractions.Remove(_context.Attractions.Single(attraction => attraction.id == tEntity.id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Attraction>> GetAll()
        {
            return await _context.Attractions.Include(a => a.city).ThenInclude(c => c.country).ToListAsync();
        }

        public async Task<ICollection<Attraction>> GetAttractionsByCity(int cityId)
        {
            return await _context.Attractions.Where(attraction => attraction.city.id == cityId).Include(c => c.city).ThenInclude(c => c.country).ToListAsync();
        }

        public async Task<ICollection<Attraction>> GetAttractionsByTypeAndCity(string type, int cityId)
        {
            return await _context.Attractions.Where(attraction => attraction.city.id == cityId && attraction.type.Equals(type)).Include(c => c.city).ThenInclude(c => c.country).ToListAsync();
        }

        public async Task<Attraction> GetById(int id)
        {
            return await _context.Attractions.Include(c => c.city).ThenInclude(c => c.country).FirstOrDefaultAsync(attraction => attraction.id == id);
        }

        public int GetLast()
        {
            var attraction = _context.Attractions.ToListAsync();
            int id = attraction.Result.Select(x => x.id).Max();
            return id;
        }

        public async Task<bool> Update(Attraction tEntity)
        {
            tEntity.city = CheckIfCreateNewCity(tEntity.city);

            if (CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                _context.Attractions.Update(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
