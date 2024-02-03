using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{

    [Url(ApiUrls.GetFull)]
    [IdQuery]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.ModelFull)]
    public class GetFullAttribute : BaseHttpAttribute
    {
    }
}
