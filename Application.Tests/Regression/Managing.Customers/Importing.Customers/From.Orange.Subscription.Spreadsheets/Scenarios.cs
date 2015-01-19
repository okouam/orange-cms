using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Customers.Importing.Customers.From.Orange.Subscription.Spreadsheets
{
    [ProvideWebApplication]
    class Scenarios
    {
        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_1()
        {
            var response = API.PostFile("/customers/import", "Orange.Subscription.csv");
            var content = (JArray)JsonConvert.DeserializeObject(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content[0].Value<string>("telephone"), Is.EqualTo("22523944"));
            Assert.That(content[0].Value<string>("speed"), Is.EqualTo("512K"));
            Assert.That(content[0].Value<string>("expiryDate"), Is.EqualTo("08/31/2014 00:00:00"));
            Assert.That(content[0].Value<string>("login"), Is.EqualTo("placide@aviso.ci"));
            Assert.That(content[0].Value<string>("status"), Is.EqualTo("Actif"));
            Assert.That(content[0].Value<string>("formula"), Is.EqualTo("ADSL ONE Regular 512 K - 2 Mois Promo Mai 2014"));
            Assert.That(content[0].Value<bool>("neverExpires"), Is.False);
        }

        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_2()
        {
            var response = API.PostJson("/customers", new {telephone = "22523944", speed = "256K", login = "anonymous@nowhere.com", formula = "None" });
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = (JObject)JsonConvert.DeserializeObject(response.Content);
            var id = content.Value<int>("id");

            response = API.PostFile("/customers/import", "Orange.Subscription.csv");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = API.Get("/customers/" + id);
            content = (JObject)JsonConvert.DeserializeObject(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(content.Value<string>("telephone"), Is.EqualTo("22523944"));
            Assert.That(content.Value<string>("speed"), Is.EqualTo("512K"));
            Assert.That(content.Value<string>("expiryDate"), Is.EqualTo("08/31/2014 00:00:00"));
            Assert.That(content.Value<string>("login"), Is.EqualTo("placide@aviso.ci"));
            Assert.That(content.Value<string>("status"), Is.EqualTo("Actif"));
            Assert.That(content.Value<string>("formula"), Is.EqualTo("ADSL ONE Regular 512 K - 2 Mois Promo Mai 2014"));
            Assert.That(content.Value<bool>("neverExpires"), Is.False);
        }
    }
}
