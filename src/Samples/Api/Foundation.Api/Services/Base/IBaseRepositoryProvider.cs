using Microsoft.Extensions.Logging;
using Foundation.Api.Services.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
