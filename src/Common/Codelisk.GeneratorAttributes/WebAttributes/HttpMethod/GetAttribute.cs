using Attributes.WebAttributes.Dto;
using Shared.Constants;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Get)]
    [IdQuery]
    [Return(Shared.Models.ReturnKind.Object)]
    public class GetAttribute : BaseHttpAttribute
    {
    }
}
