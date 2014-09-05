using System;
using System.IO;
using NUnit.Framework;
using OrangeCMS.Application.Services;

namespace OrangeCMS.Application.Tests.Services
{
    [TestFixture]
    class BoundaryServiceTests
    {
        [Test]
        public void When_generating_random_coordinates_creates_points_inside_the_boundaries()
        {
            var boundaryService = new BoundaryService();
            var shapefile = boundaryService.ExtractShapefileFromZip(Path.Combine(Environment.CurrentDirectory, "TestData", "Countries.zip"));
            var coordinates = boundaryService.GenerateRandomCoordinatesIn(shapefile, 10);
            Assert.That(coordinates.Count, Is.EqualTo(10));
        }
    }
}
