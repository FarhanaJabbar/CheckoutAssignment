using System.Threading.Tasks;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CheckoutApi.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<ActionResult<CheckoutTokenResponse>> GetApiToken(TokenRequest tokenRequest)
        {
            var token = await _authenticationService.GetToken(tokenRequest.UserName, tokenRequest.Password);
            if (token == null)
                return Unauthorized("Invalid credentials");
            return Ok(token);
        }
    }
}
