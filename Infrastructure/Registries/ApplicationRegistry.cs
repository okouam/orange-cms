using System.Configuration;
using AutoMapper;
using Codeifier.OrangeCMS.Domain.Providers;
using Codeifier.OrangeCMS.Repositories;
using OrangeCMS.Application.Controllers;
using OrangeCMS.Application.Services;
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
