using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CodeKinden.OrangeCMS.Application.Providers;
using CodeKinden.OrangeCMS.Domain.Models;
using CodeKinden.OrangeCMS.Domain.Services.Commands;
using CodeKinden.OrangeCMS.Domain.Services.Queries;
using CodeKinden.OrangeCMS.Repositories;
using CodeKinden.OrangeCMS.Tasks.Bounce;
using Newtonsoft.Json;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    class ApplicationDatabase
    {
        public static void Create()
        {
            DB.CreateOrReplaceDatabase(ConfigurationProvider.ConnectionString);
            DB.RunDatabaseMigrations(ConfigurationProvider.ConnectionString, true);
        }

        public static void CreateSnapshot()
        {
            snapshotName = DB.CreateSnapshot(ConfigurationProvider.ConnectionString);
        }

        public static void RollbackToSnapshot()
        {
            DB.RollbackToSnapshot(ConfigurationProvider.ConnectionString, snapshotName);
        }

        public static void ImportCustomers(string filename)
        {
            var customers = Import<Customer>(filename);

            if (customers == null || !customers.Any()) return;

            var service = new CustomerCommands(new DbContextScope(ConfigurationProvider.ConnectionString));
            service.Save(customers.ToArray());
        }

        public static void ImportBoundaries(string filename)
        {
            var boundaries = Import<Boundary>(filename);
            var boundaryCommands = new BoundaryCommands(new DbContextScope(ConfigurationProvider.ConnectionString));
            boundaryCommands.SaveBoundaries(boundaries);
        }

        public static void ImportUsers(string filename)
        {
            var users = Import<User>(filename);
            var dbContextScope = new DbContextScope(ConfigurationProvider.ConnectionString);
            var service = new UserCommands(new IdentityProvider(dbContextScope),  dbContextScope, new UserQueries(dbContextScope));
            service.Save(users);
        }

        private static IEnumerable<T> Import<T>(string filename)
        {
            var resourceName = Assembly.GetExecutingAssembly().GetManifestResourceNames().First(x => x.EndsWith(filename));
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            reader.Close();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
        }

        private static string snapshotName;
    }
}