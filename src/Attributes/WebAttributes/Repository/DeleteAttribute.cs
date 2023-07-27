using Attributes.WebAttributes.Repository.Base;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Attributes.WebAttributes.Repository
{
    [Url("")]
    public class DeleteAttribute : BaseHttpAttribute
    {
        public override string UrlPrefix => ApiUrls.Delete;
    }
}
