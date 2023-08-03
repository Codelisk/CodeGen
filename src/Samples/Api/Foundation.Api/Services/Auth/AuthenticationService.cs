using Foundation.Api.Models;

namespace Foundation.Api.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationService(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }

        public async Task<bool> AuthenticateAndCacheTokenAsync(AuthPayload auth)
        {
            return true;
        }

        public async Task<bool> RefreshAndCacheTokenAsync()
        {
            return true;
        }
    }
}
