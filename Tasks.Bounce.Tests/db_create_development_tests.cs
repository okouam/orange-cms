using System.Text.RegularExpressions;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using NUnit.Framework;

namespace Tasks.Bounce.Tests
{
    [TestFixture]
    public class db_create_development_tests
    {
        [Test]
        public void Works()
        {
            var task = new db_create_development();
            task.Execute("Data Source=188.165.255.78;Initial Catalog=orangecms;User=orangecms;Password=349[lks]Qas9");
        }
    }
}
