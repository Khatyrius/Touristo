using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        Task<ICollection<City>> GetCitiesByCountryWithAttractions(int countryId);
        Task<City> GetCityAttractions(int cityId);

        public Task<bool> AddAttractionToCity(int attractionId, int cityId);
        public Task<bool> RemoveAttractionFromCity(int attractionId, int cityId);
    }
}
