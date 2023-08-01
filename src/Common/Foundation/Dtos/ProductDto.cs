using Attributes;
using Foundation.Dtos.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Dtos
{
    [Dto]
    public class ProductDto : BaseDto
    {
        public string ProductName { get; set; }
    }
}
