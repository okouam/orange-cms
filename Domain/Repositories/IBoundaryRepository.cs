using System.Collections.Generic;
using OrangeCMS.Domain;

namespace Codeifier.OrangeCMS.Repositories
{
    public interface IBoundaryRepository
    {
        IEnumerable<Boundary> Save(IEnumerable<Boundary> boundaries);
    }
}