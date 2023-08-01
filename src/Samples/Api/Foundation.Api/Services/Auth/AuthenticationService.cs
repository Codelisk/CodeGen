using Foundation.Api;
using Foundation.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
