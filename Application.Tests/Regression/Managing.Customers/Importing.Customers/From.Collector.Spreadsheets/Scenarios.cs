using System.Net;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Customers.Importing.Customers.From.Collector.Spreadsheets
{
    [ProvideWebApplication]
    class Scenarios
    {
        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_1()
        {
            var response = API.PostFile("/customers/import", "Collector.csv");
            var content = (JArray)JsonConvert.DeserializeObject(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content[0].Value<string>("telephone"), Is.EqualTo("22523944"));
            Assert.That(content[0].Value<decimal>("longitude"), Is.EqualTo(-4.015684));
            Assert.That(content[0].Value<decimal>("latitude"), Is.EqualTo(5.341544));
            Assert.That(content[0].Value<string>("imageUrl"), Is.EqualTo("http://www.mydoforms.com/imageViewer?blobKey=ag9zfm15ZG9mb3Jtcy1ocmRyFwsSCmJsb2Jfc3RvcmUYgIDAvJ2J0AgM&blobName=0one$$08302014233420$$Published$$255$$P$$1.jpg"));
        }

        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_2()
        {
            var response = API.PostJson("/customers", new { telephone = "22523944"});
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var content = (JObject)JsonConvert.DeserializeObject(response.Content);
            var id = content.Value<int>("id");

            response = API.PostFile("/customers/import", "Collector.csv");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            response = API.Get("/customers/" + id);
            content = (JObject)JsonConvert.DeserializeObject(response.Content);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            Assert.That(content.Value<string>("telephone"), Is.EqualTo("22523944"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content.Value<string>("telephone"), Is.EqualTo("22523944"));
            Assert.That(content.Value<decimal>("longitude"), Is.EqualTo(-4.015684));
            Assert.That(content.Value<decimal>("latitude"), Is.EqualTo(5.341544));
            Assert.That(content.Value<string>("imageUrl"), Is.EqualTo("http://www.mydoforms.com/imageViewer?blobKey=ag9zfm15ZG9mb3Jtcy1ocmRyFwsSCmJsb2Jfc3RvcmUYgIDAvJ2J0AgM&blobName=0one$$08302014233420$$Published$$255$$P$$1.jpg"));

        }
    }
}
