using System;
using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Adding.Users.When.Administrator
{
    [ProvideWebApplication, WithAuthorizationToken(Role.Administrator)]
    class Scenarios
    {
        [Test]
        public void Scenario_1()
        {
            var symbol = Guid.NewGuid().ToString();
            var response = API.Post("/users", new { username = symbol, password = symbol, role = "Standard", email = symbol + "@nowhere.com" });
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void Scenario_2()
        {
            var symbol = Guid.NewGuid().ToString();
            var response = API.Post("/users", new { username = symbol, password = symbol, role = "Standard", email = symbol + "@nowhere.com" });
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
