using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Repositories
{
    public interface ICustomerRepository
    {
        void Save(params Customer[] customers);

        long CountAll();
    }
}