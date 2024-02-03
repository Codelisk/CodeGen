
using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Save)]
    [DtoBody]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.Model)]
    public class SaveAttribute : BaseHttpAttribute
    {
    }
}
