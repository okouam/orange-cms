using System.Configuration;
using System.Data.SqlClient;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using NUnit.Framework;

[SetUpFixture]
// ReSharper disable CheckNamespace
public class Setup
// ReSharper restore CheckNamespace
{
    [SetUp]
    public void RunBeforeAnyTests()
    {
        var task = new db_create_development();
        task.Execute(ConfigurationManager.ConnectionStrings["Main"].ConnectionString, 10, 10000);
    }
}

