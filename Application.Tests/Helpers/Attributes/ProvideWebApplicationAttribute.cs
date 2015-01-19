using System;
using System.Configuration;
using CodeKinden.OrangeCMS.Application.Tests.Regression;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    class ProvideWebApplicationAttribute : Attribute, ITestAction
    {
        public void BeforeTest(TestDetails testDetails)
        {
            API.Create(ConfigurationManager.AppSettings["regression.url"]);
        }

        public void AfterTest(TestDetails testDetails)
        {
            // do nothing
        }

        public ActionTargets Targets => ActionTargets.Default;
    }
}
