using System;
using CodeKinden.OrangeCMS.Application.Tests.Regression;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    class CleanupWebApplicationAttribute : Attribute, ITestAction
    {
        public void BeforeTest(TestDetails testDetails)
        {
            // do nothing
        }

        public void AfterTest(TestDetails testDetails)
        {
            API.Dispose();
        }

        public ActionTargets Targets => ActionTargets.Default;
    }
}
