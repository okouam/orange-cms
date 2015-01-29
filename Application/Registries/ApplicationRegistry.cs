using System.Configuration;
using AutoMapper;
using CodeKinden.OrangeCMS.Application.Endpoints.Controllers;
using CodeKinden.OrangeCMS.Domain.Services.Queries;
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
                scan.AssemblyContainingType<CustomerQueries>();
                scan.AddAllTypesOf<Profile>();
                scan.WithDefaultConventions();
            });

            For<IDbContextScope>()
                .Use(x => new DbContextScope(ConfigurationManager.ConnectionStrings["Main"].ConnectionString));
        }
    }
}
