using System.Collections.Generic;
using System.Threading.Tasks;
using DotSpatial.Topology;

namespace OrangeCMS.Domain.Services
{
    public interface IBoundaryService
    {
        Task<IEnumerable<Boundary>> GetAll();

        Task<Boundary> Get(long id);

        string ExtractShapefileFromZip(string file);

        IEnumerable<Boundary> GetBoundariesFromZip(string filename, string name, int maxBoundaries = int.MaxValue);

        IList<Coordinate> GenerateRandomCoordinatesIn(string filename, int count);

        IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string fullName);
    }
}