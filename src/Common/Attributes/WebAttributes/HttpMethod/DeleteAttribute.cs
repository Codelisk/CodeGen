
using Attributes.WebAttributes.Dto;
using Shared.Constants;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Delete)]
    [DtoBody]
    public class DeleteAttribute : BaseHttpAttribute
    {
    }
}
