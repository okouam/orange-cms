using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrangeCMS.Application;
using OrangeCMS.Persistence;
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
