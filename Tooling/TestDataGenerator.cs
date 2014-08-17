using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using OrangeCMS.Application;
using OrangeCMS.Domain;

namespace OrangeCMS.Tooling
{
    public class TestDataGenerator
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private int numUsers;
        private int numCustomers;
        private int numBoundaries;
        private int numCategories;

        public IList<Customer> Customers { get; private set; }

        public IList<Boundary> Boundaries { get; private set; }

        public IList<Category> Categories { get; private set; }

        public IList<User> Users { get; private set; }

        public TestDataGenerator SetupDatabase(DatabaseContext dbContext)
        {
            log.Info("Generating test data.");

            var client = new Client
            {
                Name = Faker.CompanyFaker.Name()
            };

            GenerateUsers(client);

            log.Info("{0} users generated.", Users.Count);

            GenerateCategories(client);

            log.Info("{0} categories generated.", Categories.Count);

            GenerateCustomers(client);

            log.Info("{0} customers generated.", Customers.Count());

            GenerateBoundaries(client);

            log.Info("{0} boundaries generated.", Boundaries.Count());

            dbContext.Clients.Add(client);

            foreach (var user in Users)
            {
                dbContext.Users.Add(user);
            }

            foreach (var category in Categories)
            {
                dbContext.Categories.Add(category);
            }

            foreach (var boundary in Boundaries)
            {
                dbContext.Boundaries.Add(boundary);
            }

            foreach (var customer in Customers)
            {
                dbContext.Customers.Add(customer);
            }

            dbContext.SaveChanges();

            log.Info("The test data has been saved to the database.");

            return this;
        }

        public TestDataGenerator WithUsers(int numUsers)
        {
            this.numUsers = numUsers;
            return this;
        }

        public TestDataGenerator WithCustomers(int numCustomers)
        {
            this.numCustomers = numCustomers;
            return this;
        }

        public TestDataGenerator WithBoundaries(int numBoundaries)
        {
            this.numBoundaries = numBoundaries;
            return this;
        }

        public TestDataGenerator WithCategories(int numCategories)
        {
            this.numCategories = numCategories;
            return this;
        }

        public User GetSpecificUser(string role)
        {
            return Users.First(x => x.Role == role);
        }

        private void GenerateBoundaries(Client client)
        {
            Boundaries = new List<Boundary>();

            numBoundaries.Times(() =>
            {
                var boundary = new Boundary
                {
                    Client = client,
                    Name = Faker.LocationFaker.City()
                };

                Boundaries.Add(boundary);
            });
        }

        private void GenerateCustomers(Client client)
        {
            Customers = new List<Customer>();

            numCustomers.Times(() =>
            {
                var customer = new Customer
                {
                    Name = Faker.NameFaker.Name(),
                    Telephone = Faker.PhoneFaker.InternationalPhone(),
                    Longitude = Faker.NumberFaker.Number(20, 120),
                    Latitude = Faker.NumberFaker.Number(20, 120),
                    Client = client,
                    CreatedBy = Users[Faker.NumberFaker.Number(0, Users.Count)],
                    Categories = Categories.Sample(0, 3).ToList()
                };

                Customers.Add(customer);
            });
        }

        private void GenerateCategories(Client client)
        {
            Categories = new List<Category>();

            numCategories.Times(() =>
            {
                var category = new Category
                {
                    Name = Faker.CompanyFaker.BS().Split(' ')[1],
                    Client = client
                };

                Categories.Add(category);
            });

            Categories = Categories.DistinctBy(x => x.Name).ToList();
        }

        private void GenerateUsers(Client client)
        {
            Users = new List<User>();

            var random = new Random();

            numUsers.Times(() =>
            {
                var user = new User
                {
                    UserName = Faker.NameFaker.FirstName(),
                    Password = Faker.StringFaker.AlphaNumeric(10),
                    Email = Faker.InternetFaker.Email(),
                    Role = Roles.All[random.Next(0, 3)],
                    Client = client
                };

                Users.Add(user);
            });
        }
    }
}
