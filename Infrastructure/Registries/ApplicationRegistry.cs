using AutoMapper;
using OrangeCMS.Application.Controllers;
using StructureMap.Configuration.DSL;

namespace GeoCMS.Infrastructure.Registries
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            Scan(scan => {
                scan.AssemblyContainingType<UsersController>();
                scan.AddAllTypesOf<Profile>();
                scan.WithDefaultConventions();
            });
        }
    }
}
