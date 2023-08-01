using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Api.Services;
using Foundation.Api.Services.Auth;
using Foundation.Api.Services.Base;

namespace Foundation.Api
{
    public static class ModuleInitializer
    {
        public static void SetupOrderlyzeApi(this IServiceCollection services)
        {
            services.SetupAuthentication();

            services.AddSingleton<IApiBuilder, ApiBuilder>();
            services.AddSingleton<IBaseRepositoryProvider, BaseRepositoryProvider>();
        }

        private static void SetupAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<ITokenProvider, TokenProvider>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
        }
    }
}
