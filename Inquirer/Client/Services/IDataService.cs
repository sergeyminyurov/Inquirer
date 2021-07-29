using Inquirer.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    public interface IDataService
    {
        Task<List<T>> GetList<T>() where T : Entity;
        Task<T> Get<T>(int id) where T : Entity;
        Task<T> AddNew<T>(T newEntity) where T : Entity;
        Task Update<T>(T entity) where T : Entity;
        Task Delete<T>(int id) where T : Entity;
        Task Swap<T>(T entity1, T entity2) where T : Entity, IOrderedEntity;
    }
}