using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Add)]
    [DtoBody]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.Model)]
    public class AddAttribute : BaseHttpAttribute
    {
    }
}
