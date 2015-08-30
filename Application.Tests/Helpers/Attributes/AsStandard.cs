using CodeKinden.OrangeCMS.Domain.Models;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests.Helpers.Attributes
{
    internal class AsStandard : TestCaseAttribute
    {
        public AsStandard() : base(new API(Role.Standard))
        {
        }
    }
}