using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.AddRange)]
    [DtoBody]
    public class AddRangeAttribute : BaseHttpAttribute
    {
    }
}
