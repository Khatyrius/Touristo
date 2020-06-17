using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Interfaces
{
    public interface ICountryRepository : IGenericRepository<Country>
    {
        Task<ICollection<City>> GetListOfCities(int countryId);

        public Task<bool> AddCityToCountry(int cityId, int countryId);
        public Task<bool> RemoveCityFromCountry(int cityId, int countryId);
    }
}
