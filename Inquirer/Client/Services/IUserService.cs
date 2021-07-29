using Inquirer.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inquirer.Services
{
    public interface IUserService
    {
        Task<List<ApplicationUser>> GetList();
        Task<ApplicationUser> Get(string id);
        Task<ApplicationUser> AddNew(ApplicationUser newEntity);
        Task<ApplicationUser> Update(ApplicationUser entity);
        Task Delete(string id);
        void Reset();
    }
}