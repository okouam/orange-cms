using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly DatabaseContext dbContext;

        public SecurityService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual User CurrentUser
        {
            get
            {
                //var id = ClaimsPrincipal.Current.FindFirst("id").Value;
                //return dbContext.Users.Find(id);               
                return dbContext.Users.Include(x => x.Client).First();
            }
        }

        public User Authenticate(string username, string password)
        {
            return this.dbContext.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
        }

        public IEnumerable<User> FindByClient(long id)
        {
            throw new System.NotImplementedException();
        }

        public User Save(User customer)
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
            var user = dbContext.Users.Find(id);
            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
        }
    }
}
