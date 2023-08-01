using Orderlyze.Service.BL;

namespace Foundation.Web.Manager.Base
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Attributes.WebAttributes.HttpMethod;
    using Attributes.WebAttributes.Repository;
    using AutoMapper;
    using Foundation.Web.Repo;
    using Foundation.Web.Repo.Base;
    using Microsoft.Extensions.Logging;

    public abstract class GetManager<T, TKey, TEntity> : BaseManager<CrudRepository<TEntity, TKey>>
        where T : class
        where TEntity : class
    {
        private readonly IMapper _mapper;

        protected GetManager(CrudRepository<TEntity, TKey> repository, IMapper mapper, ILogger logger)
            : base(repository, mapper, logger)
        {
            _mapper = mapper;
        }

        protected abstract TKey GetKey(TEntity entity);

        public async Task<IList<T>> GetAll()
        {
            return await DoWithLoggingAsync<T, TEntity>(GetAllEntities);
        }

        public async Task<int> GetCount()
        {
            return await DoWithLoggingAsync(() => Repo.GetCount());
        }
        [Get]
        public async Task<T> Get(TKey key)
        {
            return await DoWithLoggingAsyncT<T, TEntity>(() => Repo.Get(key));
        }

        public async Task<IList<T>> Get(IEnumerable<TKey> keys)
        {
            return await DoWithLoggingAsync<T, TEntity>(() => Repo.Get(keys));
        }

        [GetAll]
        protected virtual async Task<IList<TEntity>> GetAllEntities()
        {
            return await Repo.GetAll();
        }

        protected virtual async Task<T> SetDto(T dto)
        {
            return await Task.FromResult(dto);
        }

        protected virtual async Task<IEnumerable<T>> SetDto(IList<T> dtos)
        {
            foreach (var dto in dtos)
            {
                await SetDto(dto);
            }

            return dtos;
        }

        public async Task<IEnumerable<T>> MapToDto(IList<TEntity> entities)
        {
            var dtos = Mapper.Map<IEnumerable<TEntity>, IEnumerable<T>>(entities);
            return await SetDto(dtos.ToList());
        }

        public async Task<T> MapToDto(TEntity entity)
        {
            var dto = _mapper.Map<TEntity, T>(entity);
            return await SetDto(dto);
        }
    }
}