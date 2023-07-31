using Attributes.GeneratorAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Controllers
{
    [GeneratedManagerAttribute]
    public partial class CategoryManager2 : DefaultManager<CategoryDto, Guid, CategoryDto>
    {
        public CategoryManager2(CategoryRepository categoryRepository)
            : base(categoryRepository)
        {
        }
    }
}
