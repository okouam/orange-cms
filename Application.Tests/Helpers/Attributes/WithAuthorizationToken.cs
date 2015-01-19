using System;
using CodeKinden.OrangeCMS.Application.Tests.Regression;
using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    internal class WithAuthorizationTokenAttribute : Attribute, ITestAction
    {
        private readonly Role role;

        public WithAuthorizationTokenAttribute(Role role)
        {
            this.role = role;
        }

        public void BeforeTest(TestDetails testDetails)
        {
            var username = "qa-" + role.ToString("g").ToLower();
            API.GetAccessToken(username, username + "-password");
        }

        public void AfterTest(TestDetails testDetails)
        {
            // do nothing
        }

        public ActionTargets Targets => ActionTargets.Default;
    }
}
