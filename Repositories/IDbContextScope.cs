using Codeifier.OrangeCMS.Repositories;

namespace Codeifier.OrangeCMS.Domain.Providers
{
    public interface IDbContextScope
    {
        DatabaseContext CreateDbContext();
    }
}
