using System.Collections.Specialized;
using System.Net;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Authenticating.Login
{
    [TestFixture, ProvideWebApplication, WithoutAuthorizationToken, RestoreSnapshot]
    class Scenario
    {
        [Test]
        public void Scenario_1()
        {
            var response = API.PostForm("/tokens", new NameValueCollection
            {
                {"username", "qa-system"},
                {"password", "xx"},
                {"grant_type", "password"}
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Scenario_2()
        {
            var response = API.PostForm("/tokens", new NameValueCollection
            {
                {"username", "xx"},
                {"password", ConfigurationProvider.System.Password},
                {"grant_type", "password"}
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Scenario_3()
        {
            var response = API.PostForm("/tokens");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void Scenario_4()
        {
            var response = API.PostForm("/tokens", new NameValueCollection
            {
                {"username", "qa-system"},
                {"password", "qa-system-password"},
                {"grant_type", "password"}
            });

            var content = (JObject) JsonConvert.DeserializeObject(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.GetValue("token_type").Value<string>(), Is.EqualTo("bearer"));
            Assert.That(content.GetValue("access_token"), Is.Not.Null);
            Assert.That(content.GetValue("expires_in"), Is.Not.Null);
        }
    }
}
