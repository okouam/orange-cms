using System.Linq;

namespace OrangeCMS.Application.Repositories
{
    public class CustomerRepository
    {
        private readonly DatabaseContext dbContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public long CountAll(long clientId)
        {
            return dbContext.Customers.Count(x => x.Client.Id == clientId);
        }
    }
}
