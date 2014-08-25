using System.Collections.Generic;
using System.Threading.Tasks;
using DotSpatial.Topology;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> FindByClient(long id);

        Task<Customer> Save(User currentUser, Customer customer);

        Task<Customer> FindById(long id);

        Task<Customer> Update(Customer newValues);

        Task<IEnumerable<Customer>> Search(long clientId, string strMatch, long? category);

        void Delete(long id);

        Customer CreateFakeCustomer(Client client, IList<Category> categories, IList<User> users, Coordinate coordinate);
    }
}