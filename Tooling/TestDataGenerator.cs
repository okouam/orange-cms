using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using OrangeCMS.Application;
using OrangeCMS.Domain;

namespace OrangeCMS.Tooling
{
    class TestDataGenerator
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private int numUsers;
        private int numCustomers;
        private int numBoundaries;
        private int numCategories;
        private IList<Customer> customers;
        private IList<Boundary> boundaries;
        private IList<Category> categories;
        private IList<User> users;

        internal TestDataGenerator SetupDatabase(DatabaseContext dbContext)
        {
            log.Info("Generating test data.");

            var client = new Client
            {
                Name = Faker.CompanyFaker.Name()
            };

            GenerateUsers(client);

            log.Info("{0} users generated.", users.Count);

            GenerateCategories(client);

            log.Info("{0} categories generated.", categories.Count);

            GenerateCustomers(client);

            log.Info("{0} customers generated.", customers.Count());

            GenerateBoundaries(client);

            log.Info("{0} boundaries generated.", boundaries.Count());

            dbContext.Clients.Add(client);

            foreach (var user in users)
            {
                dbContext.Users.Add(user);
            }

            foreach (var category in categories)
            {
                dbContext.Categories.Add(category);
            }

            foreach (var boundary in boundaries)
            {
                dbContext.Boundaries.Add(boundary);
            }

            foreach (var customer in customers)
            {
                dbContext.Customers.Add(customer);
            }

            dbContext.SaveChanges();

            log.Info("The test data has been saved to the database.");

            return this;
        }

        internal TestDataGenerator WithUsers(int numUsers)
        {
            this.numUsers = numUsers;
            return this;
        }

        internal TestDataGenerator WithCustomers(int numCustomers)
        {
            this.numCustomers = numCustomers;
            return this;
        }

        internal TestDataGenerator WithBoundaries(int numBoundaries)
        {
            this.numBoundaries = numBoundaries;
            return this;
        }

        internal TestDataGenerator WithCategories(int numCategories)
        {
            this.numCategories = numCategories;
            return this;
        }

        internal User GetSpecificUser(string role)
        {
            return users.First(x => x.Role == role);
        }

        private void GenerateBoundaries(Client client)
        {
            boundaries = new List<Boundary>();

            numBoundaries.Times(() =>
            {
                var boundary = new Boundary
                {
                    Client = client,
                    Name = Faker.LocationFaker.City()
                };

                boundaries.Add(boundary);
            });
        }

        private void GenerateCustomers(Client client)
        {
            customers = new List<Customer>();

            numCustomers.Times(() =>
            {
                var customer = new Customer
                {
                    Name = Faker.NameFaker.Name(),
                    Telephone = Faker.PhoneFaker.InternationalPhone(),
                    Longitude = Faker.NumberFaker.Number(20, 120),
                    Latitude = Faker.NumberFaker.Number(20, 120),
                    Client = client,
                    CreatedBy = users[Faker.NumberFaker.Number(0, users.Count)],
                    Categories = categories.Sample(0, 3).ToList()
                };

                customers.Add(customer);
            });
        }

        private void GenerateCategories(Client client)
        {
            categories = new List<Category>();

            numCategories.Times(() =>
            {
                var category = new Category
                {
                    Name = Faker.CompanyFaker.BS().Split(' ')[1],
                    Client = client
                };

                categories.Add(category);
            });

            categories = categories.DistinctBy(x => x.Name).ToList();
        }

        private void GenerateUsers(Client client)
        {
            users = new List<User>();

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

                users.Add(user);
            });
        }
    }
}
