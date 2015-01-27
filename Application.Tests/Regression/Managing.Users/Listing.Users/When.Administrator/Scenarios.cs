using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Listing.Users.When.Administrator
{
    [TestFixture, ProvideWebApplication]
    class Scenarios
    {
        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_1()
        {
            var response = API.Get("/users");
            var content = (JArray) JsonConvert.DeserializeObject(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(content.Count, Is.EqualTo(4));
            Assert.That(content[0]["id"].Value<int>(), Is.EqualTo(1));
            Assert.That(content[0]["userName"].Value<string>(), Is.EqualTo("qa-administrator"));
            Assert.That(content[0]["email"].Value<string>(), Is.EqualTo("administrator@nowhere.com"));
            Assert.That(content[0]["role"].Value<string>(), Is.EqualTo("Administrator"));
        }
    }
}
