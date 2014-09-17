using System.Collections.Generic;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain;

namespace OrangeCMS.Domain.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue);

        IEnumerable<Customer> Search(string strMatch, int? boundary, int pageSize, int pageNum, bool withCoordinatesOnly);

        IEnumerable<Customer> Import(string filename);

        string Export();

        void Save(params Customer[] toArray);
    }
}