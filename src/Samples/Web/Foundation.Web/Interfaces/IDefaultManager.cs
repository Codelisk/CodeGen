using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Manager;
using Attributes.WebAttributes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Interfaces
{
    public interface IDefaultManager<TDto, TKey, TEntity> where TDto : class where TEntity : class
    {
        [Delete]
        Task Delete(TDto t);
        [Get]
        Task<TDto> Get(TKey id);
        [GetAll]
        Task<List<TDto>> GetAll();
        [Save]
        Task<TDto> Save(TDto t);
    }
}
