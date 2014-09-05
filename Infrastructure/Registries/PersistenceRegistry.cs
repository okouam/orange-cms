using Codeifier.OrangeCMS.Repositories;
using StructureMap.Configuration.DSL;

namespace GeoCMS.Infrastructure.Registries
{
    public class PersistenceRegistry : Registry
    {
        public PersistenceRegistry()
        {
            Scan(scan =>
            {
                scan.AssemblyContainingType<DatabaseContext>();
                scan.WithDefaultConventions();
            });

            For<DatabaseContext>().Use<DatabaseContext>();
        }
    }
}
