using CheckoutApi.Services;
using Microsoft.AspNetCore.DataProtection;
using Xunit;

namespace Checkout.UnitTests
{
    public class EncryptionServiceTests
    {
        [Theory]
        [InlineData("Farhana Jabbar")]
        [InlineData("Testing")]
        public void ShouldEncryptData(string stringToEncrypt)
        {
            var dataProtectionProvider = DataProtectionProvider.Create("Test");
            var encryptionService = new EncryptionService(dataProtectionProvider);

            var encryptedString = encryptionService.EncryptString(stringToEncrypt);
            var decryptedString = encryptionService.DecryptString(encryptedString);

            Assert.Equal(stringToEncrypt, decryptedString);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void ShouldNotEncryptEmptyData(string stringToEncrypt)
        {
            var dataProtectionProvider = DataProtectionProvider.Create("Test");
            var encryptionService = new EncryptionService(dataProtectionProvider);

            var encryptedString = encryptionService.EncryptString(stringToEncrypt);
            Assert.True(string.IsNullOrEmpty(encryptedString));
            var decryptedString = encryptionService.DecryptString(encryptedString);

            Assert.Equal(stringToEncrypt, decryptedString);
        }
    }
}
