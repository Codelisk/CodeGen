
using Attributes.WebAttributes.Dto;
using Shared.Constants;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Save)]
    [DtoBody]
    [Return(Shared.Models.ReturnKind.Object)]
    public class SaveAttribute : BaseHttpAttribute
    {
    }
}
