using System.Collections.Generic;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain.Services.Parameters;

namespace OrangeCMS.Domain.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAll();
        Task<User> Save(User user);
        Task<User> FindById(long id);
        Task<User> Update(long id, UpdateUserParams newValues);
        void Delete(long id);
    }
}