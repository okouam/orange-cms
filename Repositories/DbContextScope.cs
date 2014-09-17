using Codeifier.OrangeCMS.Domain.Providers;

namespace Codeifier.OrangeCMS.Repositories
{
    public class DbContextScope : IDbContextScope
    {
        readonly string connectionString;

        public DbContextScope(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DatabaseContext CreateDbContext()
        {
            return new DatabaseContext(connectionString);
        } 
    }
}
