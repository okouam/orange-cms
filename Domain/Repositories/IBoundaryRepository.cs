using System.Collections.Generic;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Repositories
{
    public interface IBoundaryRepository
    {
        IEnumerable<Boundary> Save(IEnumerable<Boundary> boundaries);

        IEnumerable<Boundary> Save(params Boundary[] boundaries);
    }
}