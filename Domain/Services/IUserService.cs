using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public interface IUserService
    {
        IList<User> GetAll();
        
        Task<User> Save(User user);
        
        Task<User> FindById(long id);
        
        Task<User> Update(long id, UpdateUserParams newValues);
        
        void Delete(long id);

        int CountAll(Func<User, bool> filter);
    }
}