namespace CheckoutApi.Services
{
    public interface IEncryptionService
    {
        string EncryptString(string dataToEncrypt);
        string DecryptString(string encryptedData);
    }
}