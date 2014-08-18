using OrangeCMS.Application;

namespace OrangeCMS.Tooling
{
    class TestDataContext : DatabaseContext
    {
        internal TestDataContext(string connectionString) : base(connectionString)
        {}
    }
}
