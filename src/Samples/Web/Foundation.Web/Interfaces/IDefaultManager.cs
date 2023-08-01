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
    public interface IDefaultManager<T, TKey, TEntity> where T : class
    {
        [Delete]
        Task Delete(T t);
        [Get]
        Task<T> Get(TKey id);
        [GetAll]
        Task<List<T>> GetAll();
        [Save]
        Task<T> Save(T t);
    }
}
