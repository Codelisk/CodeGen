using Foundation.Api.Services.Auth;
using Foundation.Api.Services.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Api
{
    public static partial class ModuleInitializer
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
