﻿using Attributes.WebAttributes.Repository.Base;
using Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attributes.WebAttributes.Repository
{
    public class GetAllAttribute : BaseHttpAttribute
    {
        public override string UrlPrefix => ApiUrls.GetAll;
    }
}
