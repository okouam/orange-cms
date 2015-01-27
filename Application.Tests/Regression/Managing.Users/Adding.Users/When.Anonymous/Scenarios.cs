using System;
using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Adding.Users.When.Anonymous
{
    [ProvideWebApplication, WithoutAuthorizationToken]
    class Scenario
    {
        [Test]
        public void Scenario_1()
        {
            var symbol = Guid.NewGuid().ToString();
            var response = API.Post("/users", new {username = symbol, password = symbol, role = "Standard", email = symbol + "@nowhere.com"});
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
