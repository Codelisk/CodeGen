using Attributes.WebAttributes.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Dtos.Base
{
    public class BaseDto
    {
        [Id]
        public Guid Id { get; set; }
    }
}
