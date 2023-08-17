using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Get)]
    [IdQuery]
    [Return(Shared.Models.ReturnKind.Object)]
    public class GetAttribute : BaseHttpAttribute
    {
    }
}
