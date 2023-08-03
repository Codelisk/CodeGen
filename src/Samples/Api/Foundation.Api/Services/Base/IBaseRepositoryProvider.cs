using Foundation.Api.Services.Auth;
using Microsoft.Extensions.Logging;

namespace Foundation.Api.Services.Base
{
    public interface IBaseRepositoryProvider
    {
        IApiBuilder GetApiBuilder();
        IAuthenticationService GetAuthenticationService();
        ILogger GetLogger();
        ITokenProvider GetTokenProvider();
    }
}
