using AutoMapper;
using NUnit.Framework;

namespace OrangeCMS.Application.Tests
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
