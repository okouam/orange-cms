using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public class UserService : IUserService
    {
        readonly IIdentityProvider identityProvider;
        readonly IDbContextScope dbContextScope;

        public UserService(IIdentityProvider identityProvider, IDbContextScope dbContextScope)
        {
            this.identityProvider = identityProvider;
            this.dbContextScope = dbContextScope;
        }

        public IList<User> GetAll()
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return dbContext.Users.ToList();
            }
        }

        public async Task<User> Save(User user)
        {
            if (user == null) throw new ArgumentNullException("user", "No user was provided when saving a user.");

            user.Password = identityProvider.CreateHash(user.Password);

            using (var dbContext = dbContextScope.CreateDbContext())
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public async Task<User> FindById(long id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                return await dbContext.Users.FindAsync(id);
            }
        }

        public async Task<User> Update(long id, UpdateUserParams newValues)
        {
            newValues.Password = identityProvider.CreateHash(newValues.Password);

            using (var dbContext = dbContextScope.CreateDbContext())
            {
                var user = dbContext.Users.Find(id);
                var entry = dbContext.Entry(user);
                entry.CurrentValues.SetValues(newValues);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public void Delete(long id)
        {
            using (var dbContext = dbContextScope.CreateDbContext())
            {
                var user = dbContext.Users.Find(id);
                dbContext.Users.Remove(user);
                dbContext.SaveChangesAsync();
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
    }
}
