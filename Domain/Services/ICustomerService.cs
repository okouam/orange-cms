using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue);

        IEnumerable<Customer> Search(string strMatch, int? boundary, int pageSize, int pageNum, bool withCoordinatesOnly);

        IEnumerable<Customer> Import(string filename, int maxCustomers = int.MaxValue);

        string Export();

        void Save(params Customer[] toArray);
    }
}