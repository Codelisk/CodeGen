using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Api.Services.Base
{
    public interface IApiBuilder
    {
        TApi BuildRestService<TApi>(Func<Task<string>>? AuthorizationHeaderValueGetter = null);
        string GetRestUrl();
    }
}
