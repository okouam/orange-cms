using System.Configuration;
using System.Data.SqlClient;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using NUnit.Framework;

[SetUpFixture]
public class Setup
{
    [SetUp]
    public void RunBeforeAnyTests()
    {
        var builder = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["Main"].ConnectionString);
        var tasks = new Tasks();
        tasks.CreateDevelopmentDatabase(builder.UserID, builder.Password, builder.DataSource, builder.InitialCatalog);
    }
}

