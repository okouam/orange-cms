using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class UserService : IUserService
    {
        public IEnumerable<User> FindByClient(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Users.Where(x => x.Id == id).Include(x => x.Client).ToList();
            }
        }

        public User Save(User user)
        {
            throw new System.NotImplementedException();
        }

        public User FindById(long id)
        {
            throw new System.NotImplementedException();
        }

        public User Update(long id, User newValues)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(long id)
        {
            using (var dbContext = new DatabaseContext())
            {
                var user = dbContext.Users.Find(id);
                dbContext.Users.Remove(user);
                dbContext.SaveChanges();
            }
        }
    }
}
