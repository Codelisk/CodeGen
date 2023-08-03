namespace Foundation.Api.Services.Base
{
    public interface IApiBuilder
    {
        TApi BuildRestService<TApi>(Func<Task<string>>? AuthorizationHeaderValueGetter = null);
        string GetRestUrl();
    }
}
