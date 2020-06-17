using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TouristoService.DataBase.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<bool> Add(TEntity tEntity);
        Task<bool> Delete(int id);
        Task<bool> Delete(TEntity tEntity);
        Task<bool> Update(TEntity tEntity);
        Task<TEntity> GetById(int id);

        Task<List<TEntity>> GetAll();

        bool CheckIfExists(int id);

        bool CheckIfExists(TEntity entity);

        public int GetLast();
    }
}
