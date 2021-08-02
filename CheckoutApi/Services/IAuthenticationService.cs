using System.Threading.Tasks;
using CheckoutApi.Models;

namespace CheckoutApi.Services
{
    public interface IAuthenticationService
    {
        Task<CheckoutTokenResponse> GetToken(string userName, string password);
    }
}