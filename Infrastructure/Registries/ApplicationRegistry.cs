using System.Configuration;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.Controllers;
using CodeKinden.OrangeCMS.Domain.Services;
using CodeKinden.OrangeCMS.Repositories;
using StructureMap.Configuration.DSL;

namespace GeoCMS.Infrastructure.Registries
{
    public class ApplicationRegistry : Registry
    {
        public ApplicationRegistry()
        {
            Scan(scan => {
                scan.AssemblyContainingType<UsersController>();
                scan.AssemblyContainingType<CustomerService>();
                scan.AddAllTypesOf<Profile>();
                scan.WithDefaultConventions();
            });

            For<IDbContextScope>()
                .Use<DbContextScope>(x => new DbContextScope(ConfigurationManager.ConnectionStrings["Main"].ConnectionString));
        }
    }
}
