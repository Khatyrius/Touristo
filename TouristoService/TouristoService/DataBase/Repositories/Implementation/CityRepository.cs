using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.DataBase.Repositories.Interfaces;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Implementation
{
    public class CityRepository : ICityRepository
    {
        private readonly DataContext _context;

        public CityRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(City tEntity)
        {
            tEntity.country = await CheckIfCreateNewCountry(tEntity.country);

            if(tEntity.attractions != null)
            {
                tEntity.attractions = FillAttractions(tEntity.attractions);
            }

            if (!CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                await _context.Cities.AddAsync(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        private ICollection<Attraction> FillAttractions(ICollection<Attraction> attractions)
        {
            List<Attraction> citysAttractions = new List<Attraction>();

            foreach (Attraction attraction in attractions)
            {
                citysAttractions.Add(CheckIfCreateNewAttraction(attraction));
            }

            return citysAttractions;
        }

        private Attraction CheckIfCreateNewAttraction(Attraction attraction)
        {
            var query = from attrac in _context.Attractions
                        where attrac.name.Equals(attraction.name, StringComparison.InvariantCultureIgnoreCase)
                        && attrac.address.Equals(attraction.address, StringComparison.InvariantCultureIgnoreCase)
                        && attrac.city.name.Equals(attraction.city.name, StringComparison.InvariantCultureIgnoreCase)
                        select attrac;

            if (query.Any())
            {
                attraction = query.First();
                return attraction;
            }

            attraction.id = 0;
            return attraction;
        }

        private async Task<Country> CheckIfCreateNewCountry(Country country)
        {
            var query = from c in _context.Countries
                        where c.name.Equals(country.name, StringComparison.InvariantCultureIgnoreCase)
                        select c;

            if (await query.AnyAsync())
            {
                country = await query.FirstAsync();
                return country;
            }

            country.id = 0;
            return country;
        }

        public async Task<bool> AddAttractionToCity(int attractionId, int cityId)
        {
            City city = GetById(cityId).Result;
            if (CheckIfExists(city))
            {
                List<Attraction> attractions = city.attractions.ToList();
                Attraction attraction = await _context.Attractions.FirstOrDefaultAsync(attraction => attraction.id == attractionId);
                if (attraction == null)
                {
                    return false;
                }

                attractions.Add(attraction);
                city.attractions = attractions;
                await Update(city);

                return true;
            }

            return false;
        }

        public bool CheckIfExists(int id)
        {
            return _context.Cities.Any(e => e.id == id);
        }

        public bool CheckIfExists(City entity)
        {
            var query = from city in _context.Cities
                        where city.name.Equals(entity.name, StringComparison.InvariantCultureIgnoreCase)
                        && city.country.name.Equals(entity.country.name, StringComparison.InvariantCultureIgnoreCase)
                        select city;

            return query.Any();
        }

        public async Task<bool> Delete(int id)
        {
            if (CheckIfExists(id))
            {
                _context.Cities.Remove(_context.Cities.Single(city => city.id == id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<bool> Delete(City tEntity)
        {
            if (CheckIfExists(tEntity.id))
            {
                _context.Cities.Remove(_context.Cities.Single(city => city.id == tEntity.id));
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<City>> GetAll()
        {
            return await _context.Cities.Include(city => city.country).Include(c => c.attractions).ToListAsync();
        }

        public async Task<City> GetById(int id)
        {
            return await _context.Cities.Include(city => city.country).Include(city => city.attractions).FirstOrDefaultAsync(city => city.id == id);
        }

        public async Task<ICollection<City>> GetCitiesByCountryWithAttractions(int countryId)
        {
            return await _context.Cities.Include(city => city.country).Include(city => city.attractions).Where(city => city.country.id == countryId).ToListAsync();
        }

        public async Task<City> GetCityAttractions(int cityId)
        {
            return await _context.Cities.Include(city => city.country).Include(city => city.attractions).FirstOrDefaultAsync(city => city.id == cityId);
        }

        public int GetLast()
        {
            var cities = _context.Cities.ToListAsync();
            int id = cities.Result.Select(x => x.id).Max();
            return id;
        }

        public async Task<bool> RemoveAttractionFromCity(int attractionId, int cityId)
        {
            City city = GetById(cityId).Result;
            if (CheckIfExists(city))
            {
                List<Attraction> attractions = city.attractions.ToList();
                Attraction attraction = await _context.Attractions.FirstOrDefaultAsync(attraction => attraction.id == attractionId);
                attractions.Remove(attraction);
                city.attractions = attractions;
                await Update(city);

                return true;
            }

            return false;
        }

        public async Task<bool> Update(City tEntity)
        {
            tEntity.country = await CheckIfCreateNewCountry(tEntity.country);

            if (tEntity.attractions != null)
            {
                tEntity.attractions = FillAttractions(tEntity.attractions);
            }

            if (CheckIfExists(tEntity.id) && !CheckIfExists(tEntity))
            {
                _context.Cities.Update(tEntity);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
