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

    internal class AsAnonymous : TestCaseAttribute
    {
    }

    internal class AsSystem : TestCaseAttribute
    {
        public AsSystem() : base(new API(Role.System))
        {
        }
    }
}
