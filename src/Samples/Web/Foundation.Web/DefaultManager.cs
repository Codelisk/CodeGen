using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Manager;
using Attributes.WebAttributes.Repository;
using AutoMapper;
using Foundation.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    [DefaultManager]
    public class DefaultManager<TDto, TKey, TEntity> : IDefaultManager<TDto,TKey,TEntity> where TDto : class where TEntity : class
    {
        private readonly IDefaultRepository<TEntity, TKey> _repo;
        private readonly IMapper _mapper;

        public DefaultManager(IDefaultRepository<TEntity, TKey> Repo, IMapper mapper)
        {
            _repo = Repo;
            _mapper = mapper;
        }
        [Delete]
        public Task Delete(TDto t)
        {
            return _repo.Delete(_mapper.Map<TEntity>(t));
        }
        [GetAll]
        public async Task<List<TDto>> GetAll()
        {
            return _mapper.Map<List<TDto>>(await _repo.GetAll());
        }
        [Save]
        public async Task<TDto> Save(TDto t)
        {
            return _mapper.Map<TDto>(await _repo.Save(_mapper.Map<TEntity>(t)));
        }
        [Get]
        public async Task<TDto> Get(TKey id)
        {
            return _mapper.Map<TDto>(await _repo.Get(id));
        }
    }
}
