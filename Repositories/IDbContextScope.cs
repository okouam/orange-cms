namespace CodeKinden.OrangeCMS.Repositories
{
    public interface IDbContextScope
    {
        DatabaseContext CreateDbContext();
    }
}
