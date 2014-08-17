using System.Collections.Generic;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface ICustomerService
    {
        IEnumerable<Customer> FindByClient(long id);

        Customer Save(User currentUser, Customer customer);

        Customer FindById(long id);

        Customer Update(Customer newValues);

        IEnumerable<Customer> Search(long clientId, string strMatch, long? category);

        void Delete(long id);
    }
}