namespace Orderlyze.Service.BL
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Foundation.Web.Manager;
    using Foundation.Web.Repo.Base;
    using Microsoft.Extensions.Logging;
    public class BaseManager<TRepo> : IManager
    {
        protected TRepo Repo { get; }
        protected IMapper Mapper { get; }
        protected ILogger Logger { get; }
      
     
        protected BaseManager(TRepo repo, IMapper mapper, ILogger logger)
        {
            Repo = repo;
            Mapper = mapper;
            Logger = logger;
        }

        protected async Task<IList<TResult>> DoWithLoggingAsync<TResult, TData>(Func<Task<IList<TData>>> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;

            try
            {
                Logger.LogTrace($"Executing {methodName} ...");

                var data = await asyncFunc();

                Logger.LogDebug($"Executed {methodName}. Returned data count: {data.Count}.");

                return Mapper.Map<IList<TResult>>(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ERROR in {GetType().Name}.{methodName}");
                throw;
            }
        }
        protected async Task<T> DoWithLoggingAsync<T>(Func<Task<T>> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;

            try
            {
                Logger.LogTrace($"Executing {methodName} ...");

                var data = await asyncFunc();

                Logger.LogDebug($"Executed {methodName}.");

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ERROR in {GetType().Name}.{methodName}");
                throw;
            }
        }
        protected async Task<T> DoWithLoggingAsyncT<T,TData>(Func<Task<TData>> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;

            try
            {
                Logger.LogTrace($"Executing {methodName} ...");

                var data = await asyncFunc();

                Logger.LogDebug($"Executed {methodName}.");

                return Mapper.Map<T>(data); 
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ERROR in {GetType().Name}.{methodName}");
                throw;
            }
        }

        protected async Task DoWithLoggingAsync(Func<Task> asyncFunc)
        {
            var methodName = asyncFunc.Method.Name;

            try
            {
                Logger.LogTrace($"Executing {methodName} ...");

                await asyncFunc();

                Logger.LogDebug($"Executed {methodName}");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"ERROR in {GetType().Name}.{methodName}");
                throw;
            }
        }
    }
}
