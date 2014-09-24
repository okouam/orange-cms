using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Parameters;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public interface IBoundaryService
    {
        Task<IEnumerable<BoundaryWithCustomerCount>> GetAll();

        Task<Boundary> Get(long id);

        string ExtractShapefileFromZip(string file);

        IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, Func<string, string> nameColumnParser, string fullName, int maxBoundaries = int.MaxValue);

        IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string fullName, int maxBoundaries = int.MaxValue);
    }
}