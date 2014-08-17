using System.Collections.Generic;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface ISecurityService
    {
        User CurrentUser { get; }

        User Authenticate(string username, string password);

        IEnumerable<User> FindByClient(long id);

        User Save(User customer);

        User FindById(long id);

        User Update(long id, User newValues);

        void Delete(long id);
    }
}
