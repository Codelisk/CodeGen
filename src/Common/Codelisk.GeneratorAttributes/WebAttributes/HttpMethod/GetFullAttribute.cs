using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{

    [Url(ApiUrls.GetFull)]
    [IdQuery]
    [Return(Shared.Models.ReturnKind.ModelFull)]
    public class GetFullAttribute : BaseHttpAttribute
    {
    }
}
