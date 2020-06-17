using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.DataBase.Repositories.Interfaces;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Implementation
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(Country tEntity)
        {
            if (tEntity.cities != null)
            {
                tEntity.cities = FillCities(tEntity.cities);
            }

            if (!CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                await _context.Countries.AddAsync(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private ICollection<City> FillCities(ICollection<City> cities)
        {
            List<City> countrysCities = new List<City>();

            foreach (City city in cities)
            {
                countrysCities.Add(CheckIfCreateNewCity(city));
            }

            return countrysCities;
        }

        private City CheckIfCreateNewCity(City entity)
        {
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

        public async Task<bool> AddCityToCountry(int cityId, int countryId)
        {
            Country country = GetById(countryId).Result;
            if (CheckIfExists(country))
            {
                List<City> cities = country.cities.ToList();
                City city = await _context.Cities.FirstOrDefaultAsync(city => city.id == cityId);
                if (city == null)
                {
                    return false;
                }

                cities.Add(city);
                country.cities = cities;
                await Update(country);

                return true;
            }

            return false;
        }

        public bool CheckIfExists(int id)
        {
            return _context.Countries.Any(e => e.id == id);
        }

        public bool CheckIfExists(Country entity)
        {
            var query = from country in _context.Countries
                        where country.name.Equals(entity.name, StringComparison.InvariantCultureIgnoreCase)
                        select country;

            return query.Any();
        }

        public async Task<bool> Delete(int id)
        {
            if (CheckIfExists(id))
            {
                _context.Countries.Remove(_context.Countries.Single(country => country.id == id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(Country tEntity)
        {
            if (CheckIfExists(tEntity.id))
            {
                _context.Countries.Remove(_context.Countries.Single(country => country.id == tEntity.id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<Country>> GetAll()
        {
            return await _context.Countries.Include(c => c.cities).ThenInclude(c => c.attractions).ToListAsync(); 
        }

        public async Task<Country> GetById(int id)
        {
            return await _context.Countries.Include(c => c.cities).ThenInclude(c => c.attractions).FirstOrDefaultAsync(country => country.id == id);
        }

        public int GetLast()
        {
            var countries = _context.Countries.ToListAsync();
            int id = countries.Result.Select(x => x.id).Max();
            return id;
        }

        public async Task<ICollection<City>> GetListOfCities(int countryId)
        {
            return await _context.Cities.Where(city => city.country.id == countryId).ToListAsync();
        }

        public async Task<bool> RemoveCityFromCountry(int cityId, int countryId)
        {
            Country country = GetById(countryId).Result;
            if (CheckIfExists(country))
            {
                List<City> cities = country.cities.ToList();
                City city = await _context.Cities.FirstOrDefaultAsync(city => city.id == cityId);
                if (city == null)
                {
                    return false;
                }

                cities.Remove(city);
                country.cities = cities;
                await Update(country);

                return true;
            }

            return false;
        }

        public async Task<bool> Update(Country tEntity)
        {
            if (tEntity.cities != null)
            {
                tEntity.cities = FillCities(tEntity.cities);
            }

            if (CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                _context.Countries.Update(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
