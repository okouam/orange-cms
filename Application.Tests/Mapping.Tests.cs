using AutoMapper;
using CodeKinden.OrangeCMS.Application.Tests.Helpers;
using NUnit.Framework;

namespace CodeKinden.OrangeCMS.Application.Tests
{
    [TestFixture]
    class MappingTests : BaseTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [Test]
        public void Check_mappings()
        {
            var mappingEngine = container.GetInstance<IMappingEngine>();
            mappingEngine.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
