using Microsoft.AspNetCore.DataProtection;

namespace CheckoutApi.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly IDataProtector _protector;
        private const string DataProtectionKey = "UjMJM3kZEw"; //key for encryption and decryption , can be moved in app settings 
        public EncryptionService(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector(DataProtectionKey);
        }

        public string EncryptString(string dataToEncrypt)
        {
            if (string.IsNullOrEmpty(dataToEncrypt)) return dataToEncrypt;
            var protectedPayload = _protector.Protect(dataToEncrypt);
            return protectedPayload;

        }

        public string DecryptString(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData)) return encryptedData;
            var unprotectedPayload = _protector.Unprotect(encryptedData);
            return unprotectedPayload;
        }
    }
}
