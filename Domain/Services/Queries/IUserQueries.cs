using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services.Queries
{
    public interface IUserQueries
    {
        IList<User> GetAll();
        Task<User> FindById(long id);
        int CountAll(Func<User, bool> filter = null);
        bool Exists(string username);
    }
}