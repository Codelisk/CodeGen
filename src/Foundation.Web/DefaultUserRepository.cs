using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    public class DefaultUserRepository<T, TKey> : DefaultRepository<T, TKey> where T : class
    {
        public DefaultUserRepository(BaseContext context) : base(context)
        {
        }
    }
}
