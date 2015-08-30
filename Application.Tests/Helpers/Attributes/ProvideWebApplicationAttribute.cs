using System;
using System.Configuration;
using Microsoft.Owin.Hosting;
using NUnit.Framework;
using RestSharp;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    class ProvideWebApplicationAttribute : Attribute, ITestAction
    {
        public void BeforeTest(TestDetails testDetails)
        {
            application = WebApp.Start<Startup>(ConfigurationManager.AppSettings["regression.url"]);
        }

        public void AfterTest(TestDetails testDetails)
        {
            if (application != null) application.Dispose();
        }

        public ActionTargets Targets
        {
            get
            {
                return ActionTargets.Default;
            }
        }

        private static IDisposable application;
    }
}
