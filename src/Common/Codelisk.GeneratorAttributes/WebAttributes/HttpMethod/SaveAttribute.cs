
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Shared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Save)]
    [DtoBody]
    [Return(Shared.Models.ReturnKind.Model)]
    public class SaveAttribute : BaseHttpAttribute
    {
    }
}
