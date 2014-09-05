using System.Linq;

namespace Codeifier.OrangeCMS.Repositories
{
    public class CustomerRepository
    {
        private readonly DatabaseContext dbContext;

        public CustomerRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public long CountAll()
        {
            return dbContext.Customers.Count();
        }
    }
}
