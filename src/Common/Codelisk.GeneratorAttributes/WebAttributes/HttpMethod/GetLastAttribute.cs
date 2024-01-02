using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.GetLast)]
    [Return(Shared.Models.ReturnKind.Model)]
    public class GetLastAttribute : BaseHttpAttribute
    {
    }
}