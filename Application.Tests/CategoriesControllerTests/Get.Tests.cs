using System;
using NUnit.Framework;

namespace OrangeCMS.Application.Tests.CategoriesControllerTests
{
    [TestFixture]
    class GetTests
    {
        [Test]
        public void When_getting_a_category_retrieves_the_correct_category()
        {
            var controller = new CategoriesController();
            var response = controller.Get(23);
            throw new NotImplementedException();
        }

        [Test]
        public void When_getting_a_category_returns_an_error_if_the_category_is_not_found()
        {
            throw new NotImplementedException();
        }
    }
}
