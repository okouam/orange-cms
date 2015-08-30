using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    internal class AsAdministrator : TestCaseAttribute
    {
        public AsAdministrator() : base(new API(Role.Administrator))
        {
        }
    }
}