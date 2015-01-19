using System.Collections.Generic;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services.Commands
{
    public interface ICustomerCommands
    {
        IEnumerable<Customer> Import(string filename, int maxCustomers = int.MaxValue);

        string Export();

        void Save(params Customer[] customers);

        void Delete(int id);
    }
}