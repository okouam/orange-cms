using System;
using System.Configuration;
using DbUp;
using DbUp.Engine;
using OrangeCMS.Application;
using OrangeCMS.Tooling.Models;

namespace OrangeCMS.Tooling
{
    class Program
    {
        public static int Main(string[] args)
        {
            Console.WriteLine("Welcome to the OrangeCMS Tooling Pack\n");
            Console.WriteLine("Please select one of the options below: \n");
            Console.WriteLine("> 1. Replace the development database with the seed database.");
            Console.WriteLine("> 2. Create the seed database.");
            
            var option = Console.ReadKey();
            Console.WriteLine("\n");

            bool isSuccess;

            var provider = new DatabaseProvider();
            var sourceConnectionString = ConfigurationManager.ConnectionStrings["Source"].ConnectionString;
            var destinationConnectionString = ConfigurationManager.ConnectionStrings["Destination"].ConnectionString;

            switch (option.Key.ToString())
            {
                case "D1":
                    isSuccess = provider.RecreateDatabaseFromTemplate(sourceConnectionString, destinationConnectionString);
                    break;
                case "D2":
                    isSuccess = new DatabaseCreator(provider).CreateSeedDatabase(sourceConnectionString, "TestData/Countries.zip").Successful;
                    break;
                default:
                    throw new Exception("Unknown option requested.");
            }
            
            Console.WriteLine("Press any key to continue...");

            Console.ReadLine();

            return isSuccess ? 0 : -1;
        }

    }
}
