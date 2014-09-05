using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OrangeCMS.Application.Providers;
using Codeifier.OrangeCMS.Domain.Services.Parameters;
using Codeifier.OrangeCMS.Repositories;

namespace OrangeCMS.Domain.Services
{
    public class UserService : IUserService
    {
        readonly IIdentityProvider identityProvider;

        public UserService(IIdentityProvider identityProvider)
        {
            this.identityProvider = identityProvider;
        }

        public async Task<List<User>> GetAll()
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Users.ToListAsync();
            }
        }

        public async Task<User> Save(User user)
        {
            if (user == null) throw new ArgumentNullException("user", "No user was provided when saving a user.");

            user.Password = identityProvider.CreateHash(user.Password);

            using (var dbContext = new DatabaseContext())
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
                return user;
            }
        }

        public async Task<User> FindById(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return await dbContext.Users.FindAsync(id);
            }
        }

        public async Task<User> Update(long id, UpdateUserParams newValues)
        {
            newValues.Password = identityProvider.CreateHash(newValues.Password);

            using (var dbContext = new DatabaseContext())
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
            using (var dbContext = new DatabaseContext())
            {
                var user = dbContext.Users.Find(id);
                dbContext.Users.Remove(user);
                dbContext.SaveChangesAsync();
            }
        }
    }
}
