using System;
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

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Default;
            }
        } 
    }
}
