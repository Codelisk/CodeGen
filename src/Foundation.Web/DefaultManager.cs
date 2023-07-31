using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Manager;
using Attributes.WebAttributes.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    [DefaultManager]
    public class DefaultManager<T, TKey, TEntity> where T : class
    {
        private readonly DefaultRepository<T, TKey> _repo;

        public DefaultManager(DefaultRepository<T, TKey> Repo)
        {
            _repo = Repo;
        }
        [Delete]
        public Task Delete(T t)
        {
            return _repo.Delete(t);
        }
        [GetAll]
        public Task<List<T>> GetAll()
        {
            return _repo.GetAll();
        }
        [Save]
        public Task<T> Save(T t)
        {
            return _repo.Save(t);
        }
        [Get]
        public Task<T> Get(TKey id)
        {
            return _repo.Get(id);
        }
    }
}
