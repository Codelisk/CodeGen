using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository.Base;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.WebAttributes.Repository
{
    [Url(ApiUrls.Get)]
    [IdQuery]
    [Return(Shared.Models.ReturnKind.Object)]
    public class GetAttribute : BaseHttpAttribute
    {
    }
}
