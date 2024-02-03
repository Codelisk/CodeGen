using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.GetLast)]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.Model)]
    public class GetLastAttribute : BaseHttpAttribute
    {
    }
}