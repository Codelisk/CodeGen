﻿using Attributes;
using Foundation.Dtos.Base;

namespace Foundation.Dtos
{
    [Dto]
    [GenerateAutoFilter]
    public partial class CategoryDto : BaseDto
    {
        public string Name { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
