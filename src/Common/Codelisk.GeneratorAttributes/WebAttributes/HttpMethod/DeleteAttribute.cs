
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Delete)]
    [IdQuery]
    public class DeleteAttribute : BaseHttpAttribute
    {
    }
}
