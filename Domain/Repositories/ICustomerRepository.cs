namespace Codeifier.OrangeCMS.Domain.Repositories
{
    public interface ICustomerRepository
    {
        void Save(params Customer[] customers);

        long CountAll();
    }
}