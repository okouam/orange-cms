using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services.Queries
{
    public interface ICustomerQueries
    {
        Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue);

        IEnumerable<Customer> Search(string strMatch, int? boundary, int pageSize, int pageNum, bool withCoordinatesOnly);

        Customer GetById(int id);
    }
}