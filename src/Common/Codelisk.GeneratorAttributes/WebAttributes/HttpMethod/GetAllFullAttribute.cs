using System;
using System.Collections.Generic;
using System.Text;
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.GetAllFull)]
    [Plural]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.ListFull)]
    public class GetAllFullAttribute : BaseHttpAttribute
    {
    }
}
