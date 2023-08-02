using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Manager;
using AutoMapper;
using Foundation.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    public class DefaultUserManager<TDto, TKey, TEntity> : DefaultManager<TDto, TKey, TEntity> where TDto : class where TEntity : class
    {
        public DefaultUserManager(IDefaultRepository<TEntity, TKey> repo, IMapper mapper) : base(repo, mapper)
        {
        }
    }
}
