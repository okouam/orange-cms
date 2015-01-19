using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Providers;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;
using CodeKinden.OrangeCMS.Domain.Services.Queries;
using CodeKinden.OrangeCMS.Repositories;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands
{
    public class UserCommands : IUserCommands
    {
        private readonly IIdentityProvider identityProvider;
        private readonly IDbContextScope dbContextScope;
        private readonly IUserQueries userQueries;

        public UserCommands(IIdentityProvider identityProvider, IDbContextScope dbContextScope, IUserQueries userQueries)
        {
            this.identityProvider = identityProvider;
            this.dbContextScope = dbContextScope;
            this.userQueries = userQueries;
        }

        public async Task<User> Save(User user)
        {
            if (user == null) throw new ArgumentNullException("user", "No user was provided when saving a user.");

            if (userQueries.Exists(user.UserName)) throw new Exception("The user already exists.");

            user.Password = identityProvider.CreateHash(user.Password);

            using (var dbContext = dbContextScope.CreateDbContext())
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public Task<User> Save(string username, string password, string email, Role role)
        {
            var user = new User { Email = email, Password = password, UserName = username, Role = Roles.FromEnum(role) };
            return Save(user);
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

        public void Save(IEnumerable<User> users)
        {
            if (users == null) throw new ArgumentNullException("users", "No users were provided when saving users.");

            using (var dbContext = dbContextScope.CreateDbContext())
            {
                foreach (var user in users)
                {
                    user.Password = identityProvider.CreateHash(user.Password);
                    dbContext.Users.Add(user);
                    dbContext.SaveChangesAsync().Wait();
                }
            }
        }
    }
}
