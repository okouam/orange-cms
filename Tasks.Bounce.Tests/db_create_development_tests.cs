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
            task.Execute("Data Source=localhost;Initial Catalog=orangecms;Integrated Security=SSPI;Type System Version=SQL Server 2012");
        }
    }
}
