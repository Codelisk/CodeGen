using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    public class DefaultUserManager<T, TKey, TEntity> : DefaultManager<T, TKey, TEntity> where T : class
    {
        public DefaultUserManager(DefaultRepository<T, TKey> Repo) : base(Repo)
        {
        }
    }
}
