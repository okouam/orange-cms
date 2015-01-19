using System;
using CodeKinden.OrangeCMS.Application.Tests.Regression;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    internal class WithoutAuthorizationTokenAttribute : Attribute, ITestAction
    {
        public void BeforeTest(TestDetails testDetails)
        {
            API.DeleteAccessToken();
        }

        public void AfterTest(TestDetails testDetails)
        {
            // do nothing
        }

        public ActionTargets Targets => ActionTargets.Default;
    }
}
