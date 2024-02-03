using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Get)]
    [IdQuery]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.Model)]
    public class GetAttribute : BaseHttpAttribute
    {
    }
}
