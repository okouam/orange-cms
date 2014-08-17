namespace OrangeCMS.Application.Tests
{
    public class TestDataContext : DatabaseContext
    {
        public TestDataContext(string connectionString) : base(connectionString)
        {}
    }
}
