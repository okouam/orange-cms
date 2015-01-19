using System;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Adding.Users.When.System.Administrator
{
    [ProvideWebApplication]
    class Scenario
    {
        [Test, WithoutAuthorizationToken]
        public void Scenario_1()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.Administrator)]
        public void Scenario_2()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.Standard)]
        public void Scenario_3()
        {
            throw new NotImplementedException();
        }

        public void Scenario_4()
        {
            throw new NotImplementedException();
        }
    }
}
