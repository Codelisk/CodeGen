using Codelisk.GeneratorAttributes.WebAttributes.Dto;
using Codelisk.GeneratorShared.Constants;

namespace Codelisk.GeneratorAttributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.GetAll)]
    [Plural]
    [Return(Codelisk.GeneratorShared.Models.ReturnKind.List)]
    public class GetAllAttribute : BaseHttpAttribute
    {
    }
}
