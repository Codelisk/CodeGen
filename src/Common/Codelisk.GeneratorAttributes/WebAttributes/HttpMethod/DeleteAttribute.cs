
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Delete)]
    [IdQuery]
    public class DeleteAttribute : BaseHttpAttribute
    {
    }
}
