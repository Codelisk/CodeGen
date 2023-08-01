namespace Foundation.Web.Manager.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.JsonPatch;
    using Microsoft.Extensions.Logging;
    using Foundation.Web.Repo.Base;
    using Foundation.Web.Extensions;
    using Attributes.WebAttributes.Repository;
    using Attributes.WebAttributes.Manager;
    using Orderlyze.Service.DL.Base;
    using Foundation.Web.Repo;
    using Attributes.WebAttributes.HttpMethod;

    public abstract class CrudManager<T, TKey, TEntity> : GetManager<T, TKey, TEntity>, ICrudManager<T, TKey>
        where T : class
        where TEntity : class
        where TKey : IComparable
    {
        #region private /ctr

        private readonly TKey _minimumValue = default;
        private readonly IMapper _mapper;
        private readonly CrudRepository<TEntity, TKey> _repository;

        protected CrudManager(CrudRepository<TEntity, TKey> repository, IMapper mapper, ILogger logger)
            : base(repository, mapper, logger)
        {
            _repository = repository;
            _mapper = mapper;
        }

        #endregion

        #region Add

        public async Task<TKey> Add(T value)
        {
            return (await Add(new List<T> { value })).First();
        }

        public async Task<IEnumerable<TKey>> Add(IEnumerable<T> values)
        {
            var myValues = values.ToICollection();

            await ValidateDto(myValues, ValidationType.AddValidation);
            var entities = MapFromDtos(myValues, ValidationType.AddValidation).ToICollection();

            foreach (var entity in entities)
            {
                AddEntity(entity);
            }

            _repository.AddRange(entities);

            await _repository.SaveChangesAsync();

            return entities.Select(GetKey);

        }

        [Save]
        public async Task<T> Set(T value)
        {
            await ValidateDto(value, ValidationType.SetValidation);
            var entity = MapFromDto(value, ValidationType.SetValidation);


            if (typeof(TKey) == typeof(int) || typeof(TKey) == typeof(long) || typeof(TKey) == typeof(short))
            {
                if (GetKey(entity).CompareTo(_minimumValue) == 0)
                {
                    AddEntity(entity);
                    _repository.Add(entity);
                }
                else
                {
                    var entityInDb = await _repository.GetTracking(GetKey(entity));
                    if (entityInDb == null)
                    {
                        AddEntity(entity);
                        _repository.Add(entity);
                    }
                    else
                    {
                        await Modifying(entityInDb, entity);
                        UpdateEntity(entityInDb, entity);
                    }
                }

            }
            else
            {
                var entityInDb = await _repository.GetTracking(GetKey(entity));
                if (entityInDb == null)
                {
                    AddEntity(entity);
                    _repository.Add(entity);
                }
                else
                {
                    await Modifying(entityInDb, entity);
                    UpdateEntity(entityInDb, entity);
                }
            }
            await _repository.SaveChangesAsync();
            await Modified();
            return await DoWithLoggingAsyncT<T, TEntity>(() => Task.FromResult(entity));
        }

        #endregion

        #region Delete

        [Delete]
        public async Task Delete(T value)
        {
            await Delete(new[] { value });
        }

        public async Task Delete(IEnumerable<T> values)
        {
            var myValues = values.ToICollection();

            await ValidateDto(myValues, ValidationType.DeleteValidation);
            var entities = MapFromDtos(myValues, ValidationType.DeleteValidation).ToICollection();

            foreach (var entity in entities)
            {
                DeleteEntity(entity);
            }

            _repository.DeleteRange(entities);
            await _repository.SaveChangesAsync();

        }

        public async Task Delete(TKey key)
        {
            await Delete(new[] { key });
        }

        public async Task Delete(IEnumerable<TKey> keys)
        {
            var entities = await _repository.GetTracking(keys);

            foreach (var entity in entities)
            {
                DeleteEntity(entity);
            }

            _repository.DeleteRange(entities);

            await _repository.SaveChangesAsync();
        }

        #endregion

        #region Update

        public async Task Update(T value)
        {
            await Update(new[] { value });
        }

        public async Task Update(IEnumerable<T> values)
        {
            var myValues = values.ToICollection();
            await ValidateDto(myValues, ValidationType.UpdateValidation);

            var entities = MapFromDtos(myValues, ValidationType.UpdateValidation).ToICollection();
            var entitiesInDb = await _repository.GetTracking(entities.Select(GetKey));

            await Update(myValues, entitiesInDb, entities);
            await _repository.SaveChangesAsync();
        }

        protected async Task Update(IEnumerable<T> values, IList<TEntity> entitiesInDb, IEnumerable<TEntity> entities)
        {
            var myEntities = entities.ToICollection();
            var myValues = values.ToICollection();

            var mergeJoin = entitiesInDb.Join(myEntities, GetKey, GetKey, (entityInDb, entity) => new { EntityInDb = entityInDb, Entity = entity }).ToList();

            if (myEntities.Count != entitiesInDb.Count || myEntities.Count != mergeJoin.Count)
            {
                throw new ArgumentException("Join Result diffrent");
            }

            foreach (var merged in mergeJoin)
            {
                UpdateEntity(myValues, merged.EntityInDb, merged.Entity);
            }

            await Task.CompletedTask;
        }

        #endregion

        #region Patch

        public async Task Patch(TKey key, JsonPatchDocument<T> patch)
        {
            var value = await Get(key);

            if (value == null)
            {
                throw new ArgumentException("Object was not found");
            }

            ApplyToPatch(value, patch);
            await Update(value);
        }

        protected virtual void ApplyToPatch(T dto, JsonPatchDocument<T> patch)
        {
            patch.ApplyTo(dto);
        }

        #endregion


        #region Validadation and Modification overrides

        protected enum ValidationType
        {
            AddValidation,
            SetValidation,
            UpdateValidation,
            DeleteValidation
        }

        protected virtual async Task ValidateDto(IEnumerable<T> values, ValidationType validation)
        {
            foreach (var dto in values)
            {
                await ValidateDto(dto, validation);
            }
        }

#pragma warning disable 1998
        protected virtual async Task ValidateDto(T dto, ValidationType validation)
        {
        }

        protected virtual void AddEntity(TEntity entity)
        {
        }

        protected virtual void DeleteEntity(TEntity entityInDb)
        {
        }

        protected virtual void UpdateEntity(IEnumerable<T> dtos, TEntity entityInDb, TEntity entity)
        {
            UpdateEntity(entityInDb, entity);
        }

        protected virtual void UpdateEntity(TEntity entityInDb, TEntity values)
        {
            _repository.SetValueGraph(entityInDb, values);
        }

        protected virtual async Task Modifying(TEntity entity, TEntity values)
        {
        }

        protected virtual async Task Modified()
        {
        }
#pragma warning restore 1998

        protected virtual IEnumerable<TEntity> MapFromDtos(IEnumerable<T> values, ValidationType validation)
        {
            return _mapper.Map<IEnumerable<T>, IEnumerable<TEntity>>(values);
        }
        protected virtual TEntity MapFromDto(T values, ValidationType validation)
        {
            return _mapper.Map<T, TEntity>(values);
        }

        #endregion
    }
}