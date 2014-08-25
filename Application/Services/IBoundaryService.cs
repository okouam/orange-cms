using System.Collections.Generic;
using System.Threading.Tasks;
using DotSpatial.Topology;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Services
{
    public interface IBoundaryService
    {
        Task<IEnumerable<Boundary>> FindByClient(long id);

        Task<IEnumerable<Boundary>> SaveBoundariesInZip(string nameColumn, string filename, long clientId);

        Task<Boundary> Get(long id);

        string ExtractShapefileFromZip(string file);

        IEnumerable<Boundary> GetBoundariesFromZip(string filename, string name);

        IList<Coordinate> GenerateRandomCoordinatesIn(string filename, int count);
    }
}