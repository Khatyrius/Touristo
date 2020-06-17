using System.Collections.Generic;
using System.Threading.Tasks;
using TouristoService.Models;

namespace TouristoService.DataBase.Repositories.Interfaces
{
    public interface IAttractionRepostiory : IGenericRepository<Attraction>
    {
        Task<ICollection<Attraction>> GetAttractionsByTypeAndCity(string type, int cityId);
        Task<ICollection<Attraction>> GetAttractionsByCity(int cityId);
    }
}
