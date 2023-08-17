using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Foundation.Api.Services.Base;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using System.Diagnostics;
using System.Net;

namespace Foundation.Api.Base
{
    [Codelisk.GeneratorAttributes.ApiAttributes.DefaultApiRepository]
    public abstract class BaseRepository<TApi> where TApi : IBaseApi
    {
        private const int MAX_REFRESH_TOKEN_ATTEMPTS = 1;

        protected readonly AsyncRetryPolicy _refreshTokenPolicy;

        protected readonly TApi _repositoryApi;

        private readonly JsonSerializerSettings _jsonSettings;
        private readonly ILogger _logger;
        private readonly IBaseRepositoryProvider _baseRepositoryProvider;

        protected BaseRepository(IBaseRepositoryProvider baseRepositoryProvider)
        {
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.TypeNameHandling = TypeNameHandling.All;

            _repositoryApi = baseRepositoryProvider.GetApiBuilder().BuildRestService<TApi>(GetAuthorizationHeaderValueAsync);
            _logger = baseRepositoryProvider.GetLogger();

            _refreshTokenPolicy = Policy
                .HandleInner<Refit.ApiException>(ex => ex.StatusCode == HttpStatusCode.Unauthorized || ex.StatusCode == HttpStatusCode.Forbidden)
                .RetryAsync(MAX_REFRESH_TOKEN_ATTEMPTS, RefreshAuthorizationAsync);
            _baseRepositoryProvider = baseRepositoryProvider;
        }

        /// <summary>
        /// Here we refresh the current authorization token
        /// </summary>
        /// <param name="error">Error from refreshToken policy</param>
        /// <param name="attempt">current attempt</param>
        /// <returns>Task</returns>
        protected virtual async Task RefreshAuthorizationAsync(Exception error, int attempt)
        {
            await _baseRepositoryProvider.GetAuthenticationService().RefreshAndCacheTokenAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Here we provide the Auth token for requests
        /// </summary>
        /// <returns>Current access token</returns>
        protected virtual Task<string> GetAuthorizationHeaderValueAsync()
        {
            return Task.FromResult(_baseRepositoryProvider.GetTokenProvider().GetCurrentAccessToken());
        }

        /// <summary>
        /// Do an apu request with Try catch logic and resiciance policy
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="func">Api request function</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Request result or default value when exception handle</returns>
        [Save]
        [Get]
        [GetAll]
        protected virtual async Task<T> TryRequest<T>(Func<Task<T>> func, T defaultValue = default(T))
        {
            try
            {
                return await RequestWithPolicy(func).ConfigureAwait(false);
            }
            catch (Refit.ApiException ex)
            {
                PrintExceptionMessage(ex);
                throw ex;
            }
        }
        [Delete]
        protected Task JustSend(Func<Task> task)
        {
            return task.Invoke();
        }

        /// <summary>
        /// Do an api request with resiliance policy thats will retry unitl max attemps
        /// </summary>
        /// <typeparam name="T">Result type</typeparam>
        /// <param name="func">Api request function</param>
        /// <returns>Request result or ApiException</returns>
        protected virtual async Task<T> RequestWithPolicy<T>(Func<Task<T>> func) =>
            await _refreshTokenPolicy.ExecuteAsync(func).ConfigureAwait(false);

        /// <summary>
        /// Request attempt delay logic
        /// </summary>
        /// <param name="attempt">Current attempt</param>
        /// <returns>Delay to wait for next wait</returns>
        protected virtual TimeSpan SleepDuration(int attempt) =>
            TimeSpan.FromSeconds(Math.Pow(2, attempt));

        /// <summary>
        /// Filter for resiliance
        /// </summary>
        /// <param name="ex">Api exception to get statuscode</param>
        /// <returns>True if we can do an attempt</returns>
        private static bool StatusCodeFilter(Refit.ApiException ex) =>
            ex.StatusCode != HttpStatusCode.NotFound && ex.StatusCode != HttpStatusCode.Forbidden;

        private static void PrintExceptionMessage(Refit.ApiException ex)
        {
            Debug.WriteLine("APIEXCEPTION");
            Debug.WriteLine(ex);
            Debug.WriteLine(ex.Content);
            Debug.WriteLine(ex.RequestMessage?.RequestUri?.AbsoluteUri);
        }
    }
}
