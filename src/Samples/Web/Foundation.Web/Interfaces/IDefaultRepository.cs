using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web.Interfaces
{
    public interface IDefaultRepository<TEntity, TKey> where TEntity : class
    {
        [Delete]
        Task Delete(TEntity t);
        [Get]
        Task<TEntity> Get(TKey id);
        [GetAll]
        Task<List<TEntity>> GetAll();
        [Save]
        Task<TEntity> Save(TEntity t);
    }
}
