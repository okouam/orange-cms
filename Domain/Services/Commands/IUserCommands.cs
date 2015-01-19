using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands
{
    public interface IUserCommands
    {
        Task<User> Save(User user);
        Task<User> Save(string username, string password, string email, Role role);
        Task<User> Update(long id, UpdateUserParams newValues);
        void Delete(long id);
        void Save(IEnumerable<User> users);
    }
}