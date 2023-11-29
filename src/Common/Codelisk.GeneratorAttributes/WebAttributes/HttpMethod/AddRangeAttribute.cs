using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.AddRange)]
    [DtoBodyList]
    public class AddRangeAttribute : BaseHttpAttribute
    {
    }
}
