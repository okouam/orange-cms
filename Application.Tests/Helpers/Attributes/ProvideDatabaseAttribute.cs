using System;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    class ProvideDatabaseAttribute : Attribute, ITestAction
    {
        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Default;
            }
        } 

        public void BeforeTest(TestDetails testDetails)
        {
            ApplicationDatabase.Create();

            ApplicationDatabase.ImportUsers("Users.json");

            ApplicationDatabase.ImportBoundaries("Boundaries.json");

            ApplicationDatabase.ImportCustomers("Customers.json");

            ApplicationDatabase.CreateSnapshot();
        }

        public void AfterTest(TestDetails testDetails)
        {
            // do nothing
        }
    }
}
