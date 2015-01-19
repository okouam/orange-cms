using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public interface IBoundaryQueries
    {
        IEnumerable<Boundary> GetAll();

        Task<Boundary> Get(long id);
    }
}