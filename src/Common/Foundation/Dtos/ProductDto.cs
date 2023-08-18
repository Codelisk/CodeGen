using Codelisk.GeneratorAttributes;
using Foundation.Dtos.Base;

namespace Foundation.Dtos
{
    [Dto]
    public class ProductDto : BaseDto
    {
        public string ProductName { get; set; }
    }
}
