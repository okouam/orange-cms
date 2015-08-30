using System;
using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Adding.Users.When.Anonymous
{
    [ProvideWebApplication]
    class Scenario
    {
        [AsAnonymous]
        public void Scenario_1(API api)
        {
            var symbol = Guid.NewGuid().ToString();
            var response = api.Post("/users", new {username = symbol, password = symbol, role = "Standard", email = symbol + "@nowhere.com"});
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
