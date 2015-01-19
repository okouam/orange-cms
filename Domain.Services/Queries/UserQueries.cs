using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Domain.Services.Queries
{
    public class UserQueries : IUserQueries
    {
        private readonly IDbContextScope dbContextScope;

        public UserQueries(IDbContextScope dbContextScope)
        {
            this.dbContextScope = dbContextScope;
        }

        public IList<User> GetAll()
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return dbContext.Users.ToList();
            }
        }

        public async Task<User> FindById(long id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return await dbContext.Users.FindAsync(id);
            }
        }

        public int CountAll(Func<User, bool> filter = null)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                var query = dbContext.Users.AsQueryable();

                if (filter != null)
                {
                    query = query.Where(filter).AsQueryable();
                }

                return query.Count();
            }
        }

        public bool Exists(string username)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return dbContext.Users.FirstOrDefault(x => x.UserName == username) != null;
            }
        }
    }
}
