using System.Collections.Generic;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface IUserService
    {
        IEnumerable<User> FindByClient(long id);
        User Save(User user);
        User FindById(long id);
        User Update(long id, User newValues);
        void Delete(long id);
    }
}