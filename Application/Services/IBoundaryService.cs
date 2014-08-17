using System.Collections.Generic;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface IBoundaryService
    {
        IEnumerable<Boundary> FindByClient(long id);
    }
}