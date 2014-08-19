using System.Data.Entity;
using System.Linq;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Providers
{
    public class IdentityProvider : IIdentityProvider
    {
        public virtual User User
        {
            get
            {
                using (var dbContext = new DatabaseContext())
                {
                    return dbContext.Users.Include(x => x.Client).First();
                }
            }
        }

        public User Authenticate(string username, string password)
        {
            using (var dbContext = new DatabaseContext())
            {
                return dbContext.Users.FirstOrDefault(x => x.UserName == username && x.Password == password);
            }
        }
    }
}
