using NUnit.Framework;
using OrangeCMS.Application.Providers;

namespace OrangeCMS.Application.Tests.Providers
{
    class IdentityProviderTests
    {
        [Test]
        public void When_creating_a_hash_creates_different_hashes_for_the_same_password()
        {
            var identityProvider = new IdentityProvider();
            var hash1 = identityProvider.CreateHash("mytestpassword");
            var hash2 = identityProvider.CreateHash("mytestpassword");
            Assert.That(hash1, Is.Not.EqualTo(hash2));
        }

        [Test]
        public void When_validation_passwords_correctly_identifies_correct_passwords()
        {
            var identityProvider = new IdentityProvider();
            var hash1 = identityProvider.CreateHash("mytestpassword");
            Assert.That(identityProvider.ValidatePassword("mytestpassword", hash1), Is.True);
        }
    }
}
