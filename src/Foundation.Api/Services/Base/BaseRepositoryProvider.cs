using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Foundation.Api.Services.Auth;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Services.Base
{
    public class BaseRepositoryProvider : IBaseRepositoryProvider
    {
        private readonly ITokenProvider _authTokenProvider;
        private readonly IApiBuilder _apiBuilder;
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<IBaseRepositoryProvider> _logger;

        public BaseRepositoryProvider(
            IApiBuilder apiBuilder,
            ITokenProvider tokenProvider,
            IAuthenticationService authenticationService,
            ILogger<IBaseRepositoryProvider> logger)
        {
            _authTokenProvider = tokenProvider;
            _apiBuilder = apiBuilder;
            _authenticationService = authenticationService;
            _logger = logger;
        }
        public ITokenProvider GetTokenProvider()
        {
            return _authTokenProvider;
        }
        public IAuthenticationService GetAuthenticationService()
        {
            return _authenticationService;
        }
        public IApiBuilder GetApiBuilder()
        {
            return _apiBuilder;
        }
        public ILogger GetLogger()
        {
            return _logger;
        }
    }
}
