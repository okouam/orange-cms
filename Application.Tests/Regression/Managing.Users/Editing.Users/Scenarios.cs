using System;
using CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes;
using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Regression.Managing.Users.Editing.Users
{
    [ProvideWebApplication]
    class Scenarios
    {
        [Test, WithAuthorizationToken(Role.Administrator), RestoreSnapshot]
        public void Scenario_1()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.System), RestoreSnapshot]
        public void Scenario_2()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.Standard), RestoreSnapshot]
        public void Scenario_3()
        {
            throw new NotImplementedException();
        }

        [Test, WithoutAuthorizationToken]
        public void Scenario_4()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.Standard), RestoreSnapshot]
        public void Scenario_5()
        {
            throw new NotImplementedException();
        }

        [Test, WithAuthorizationToken(Role.Standard), RestoreSnapshot]
        public void Scenario_6()
        {
            throw new NotImplementedException();
        }
    }
}
