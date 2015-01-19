using System;
using System.Collections.Generic;
using CodeKinden.OrangeCMS.Domain.Models;

namespace CodeKinden.OrangeCMS.Domain.Services
{
    public interface IBoundaryCommands
    {
        string ExtractShapefileFromZip(string file);

        IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, Func<string, string> nameColumnParser, string fullName, int maxBoundaries = int.MaxValue);

        IEnumerable<Boundary> SaveBoundariesInZip(string nameColumn, string fullName, int maxBoundaries = int.MaxValue);
    }
}