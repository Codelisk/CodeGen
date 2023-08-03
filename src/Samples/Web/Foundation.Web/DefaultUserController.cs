using Attributes.WebAttributes.Controller;
using Attributes.WebAttributes.Dto;
using Attributes.WebAttributes.Manager;
using Attributes.WebAttributes.Repository;
using Foundation.Dtos.Base;
using Foundation.Web.Interfaces;
using Foundation.Web.Manager.Base;
using Orderlyze.Service.BL.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    [DefaultController]
    [UserDto]
    public class DefaultUserController<T, TKey, TEntity> : DefaultController<T, TKey, TEntity>
        where T : BaseDto
        where TEntity : class
        where TKey : IComparable
    {
        public DefaultUserController(IDefaultManager<T, TKey, TEntity> manager) : base(manager)
        {
        }
    }
}
