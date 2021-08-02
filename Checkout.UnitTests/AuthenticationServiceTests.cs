using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CheckoutApi.Models;
using CheckoutApi.Services;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Checkout.UnitTests
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IOptions<AuthSettings>> _authSettings = new();
        readonly Mock<ILoggerWrapper<AuthenticationService>> _logger = new();

        public AuthenticationServiceTests()
        {
            _authSettings.Setup(x => x.Value).Returns(new AuthSettings()
            {
                Authority = "https://dev-901rcatx.auth0.com/",
                ClientId = "I11MCVc91ecS6xs07HZZEX5LblBE5HzI",
                ClientSecret = "G8o0N8HKP_574f-70uFhZicnq3isTWg26B-Y4b06TZfNJLIuAg7bCGXbbfwC5bOy",
                Audience = "https://localhost:44306/",
                TokenUrl = "https://dev-901rcatx.auth0.com/oauth/token"
            });
        }

        [Fact]
        public async Task Should_Return_Token_For_Valid_Cred()
        {
            var httpClientWrapper = new HttpClientWrapper();
            
            var authService = new AuthenticationService(_authSettings.Object, httpClientWrapper, _logger.Object);

            var authResult = await authService.GetToken("Farhana.Jabbar", "Password123!");

            Assert.NotNull(authResult);
            Assert.NotNull(authResult.AccessToken);
            Assert.False(string.IsNullOrEmpty(authResult.AccessToken));
            Assert.Equal("Bearer", authResult.TokenType, StringComparer.CurrentCultureIgnoreCase);
        }

        [Fact]
        public async Task Should_Return_Null_For_InValid_Cred()
        {
            var httpClientWrapper = new HttpClientWrapper();
            var error = string.Empty;
            var authService = new AuthenticationService(_authSettings.Object, httpClientWrapper, _logger.Object);
            _logger.Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Callback(
                (string errorMessage, Exception ex) =>
                {
                    error = errorMessage;
                });

            var authResult = await authService.GetToken("Farhana.Jabbar", "WrongPassword");

            Assert.Null(authResult);
            Assert.True(!string.IsNullOrEmpty(error));
        }
    }
}
