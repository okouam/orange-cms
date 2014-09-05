using System.Collections.Generic;
using System.Threading.Tasks;
using Codeifier.OrangeCMS.Domain;
using DotSpatial.Topology;
using Codeifier.OrangeCMS.Domain.Services.Parameters;

namespace OrangeCMS.Domain.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAll(int numCustomers = int.MaxValue);

        Task<Customer> Save(Customer customer);

        Task<Customer> FindById(long id);

        Task<Customer> Update(long id, UpdateCustomerParams newValues);

        void Delete(long id);

        Customer CreateFakeCustomer(IList<User> users, Coordinate coordinate);

        Task<IEnumerable<Customer>> Search(string strMatch, int pageSize, int pageNum);

        Task<IEnumerable<Customer>> Import(string filename);
    }
}