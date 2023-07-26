using Foundation.Dtos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    public static class ModuleInitializer
    {
        public static void AddFoundationWeb(this IServiceCollection Services)
        {
            Services.AddTransient<BaseContext<CategoryDto>>();
            Services.AddTransient<DefaultRepository<CategoryDto>>();
        }
    }
}
