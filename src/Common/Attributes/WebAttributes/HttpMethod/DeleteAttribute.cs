
using Attributes.WebAttributes.Dto;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.HttpMethod
{
    [Url(ApiUrls.Delete)]
    [DtoBody]
    public class DeleteAttribute : BaseHttpAttribute
    {
    }
}
