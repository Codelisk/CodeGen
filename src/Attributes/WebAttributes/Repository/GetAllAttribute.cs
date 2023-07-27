using Attributes.WebAttributes.Repository.Base;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.WebAttributes.Repository
{
    [Url(ApiUrls.GetAll)]
    [Plural]
    [Return(Shared.Models.ReturnKind.List)]
    public class GetAllAttribute : BaseHttpAttribute
    {
    }
}
