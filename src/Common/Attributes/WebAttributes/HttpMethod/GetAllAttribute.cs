using Attributes.WebAttributes.Dto;
using Shared.Constants;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.GetAll)]
    [Plural]
    [Return(Shared.Models.ReturnKind.List)]
    public class GetAllAttribute : BaseHttpAttribute
    {
    }
}
