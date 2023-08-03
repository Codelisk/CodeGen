using Foundation.Api.Models;

namespace Foundation.Api.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<bool> AuthenticateAndCacheTokenAsync(AuthPayload auth);
        Task<bool> RefreshAndCacheTokenAsync();
    }
}
