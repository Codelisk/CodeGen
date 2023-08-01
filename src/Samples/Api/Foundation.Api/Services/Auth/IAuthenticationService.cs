using Foundation.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAndCacheTokenAsync(AuthPayload auth);
        Task<bool> RefreshAndCacheTokenAsync();
    }
}
