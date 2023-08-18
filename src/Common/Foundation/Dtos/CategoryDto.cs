using Codelisk.GeneratorAttributes;
using Foundation.Dtos.Base;

namespace Foundation.Dtos
{
    [Dto]
    public partial class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
