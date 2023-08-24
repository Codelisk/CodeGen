using Newtonsoft.Json;
using Refit;

namespace Foundation.Api.Services.Base
{
    public class ApiBuilder : IApiBuilder
    {
        private readonly JsonSerializerSettings _jsonSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public TApi BuildRestService<TApi>(Func<Task<string>>? AuthorizationHeaderValueGetter = null)
        {
            return RestService.For<TApi>(GetRestUrl(), BuildApiSettings(AuthorizationHeaderValueGetter));
        }

        /// <summary>
        /// Build api settings
        /// </summary>
        /// <returns>Refit Settings</returns>
        private RefitSettings BuildApiSettings(Func<Task<string>>? AuthorizationHeaderValueGetter = null)
        {
            return new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(_jsonSettings)
            };
        }

        public string GetRestUrl()
        {
            return "";
        }
    }
}
