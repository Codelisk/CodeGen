using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [GeneratedManagerAttribute]
    [RegisterTransient]
    public partial class CategoryManager1 : DefaultManager<CategoryDto, Guid, Category>, ICategoryManager
    {
        public CategoryManager1(IDefaultRepository<Category, Guid> categoryRepository, IMapper mapper)
            : base(categoryRepository, mapper)
        {
        }
    }

    public partial interface ICategoryManager : IDefaultManager<CategoryDto, Guid, Category>
    {
    }
}
