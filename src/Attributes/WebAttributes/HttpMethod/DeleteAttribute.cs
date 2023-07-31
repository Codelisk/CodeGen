using Attributes.WebAttributes.Repository.Base;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    [Url(ApiUrls.Delete)]
    [DtoBody]
    public class DeleteAttribute : BaseHttpAttribute
    {
    }
}
