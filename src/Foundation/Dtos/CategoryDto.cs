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
    public partial class CategoryDto : BaseDto
    {
        public string Name { get; set; }
    }
}
