
using Attributes.WebAttributes.Dto;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Save)]
    [DtoBody]
    [Return(Shared.Models.ReturnKind.Object)]
    public class SaveAttribute : BaseHttpAttribute
    {
    }
}
