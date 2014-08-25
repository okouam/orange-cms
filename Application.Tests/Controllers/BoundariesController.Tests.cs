using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using NUnit.Framework;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Domain;

namespace OrangeCMS.Application.Tests.Controllers
{
    [TestFixture]
    internal class BoundariesControllerTests : BaseTest
    {
        BoundariesController controller;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            FakeIdentityProvider.AssignCurrentUser(GetSpecificUser(Roles.Standard));
            controller = container.GetInstance<BoundariesController>();
        }

        [Test]
        public async void When_getting_all_boundaries_retrieves_all_available()
        {
            var boundaryCount = GetDatabaseContext().Boundaries.Count();
            var boundaries = await controller.GetAll();
            Assert.That(boundaryCount, Is.EqualTo(boundaries.Count));
        }

        [Test]
        public async void When_getting_a_boundary_returns_the_WKT()
        {
            var boundary = GetDatabaseContext().Boundaries.First();
            var result = await controller.Get(boundary.Id);
            Assert.That(boundary.WKT, Is.EqualTo(result.WKT));
            Assert.That(boundary.Id, Is.EqualTo(result.Id));
            Assert.That(boundary.Name, Is.EqualTo(result.Name));
        }

        [Test]
        public async void When_creating_a_boundary_stores_it_in_the_database()
        {
            var content = CreateMultipartFormDataContent("Countries.zip");
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "boundaries") { Content = content };
            var boundaries = await controller.Create("name");
            Assert.That(boundaries.Count, Is.EqualTo(177));
        }

        private MultipartFormDataContent CreateMultipartFormDataContent(string filename)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(GetFileBytes(filename));
            var contentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = Guid.NewGuid() .ToString()
            };
            fileContent.Headers.ContentDisposition = contentDisposition;
            content.Add(fileContent);
            return content;
        }

        private byte[] GetFileBytes(string filename)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "TestData", filename);
            return File.ReadAllBytes(path);
        }
    }
}
