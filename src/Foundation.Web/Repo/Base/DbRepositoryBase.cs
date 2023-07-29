namespace Orderlyze.Service.DL.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Foundation.Web.Repo.Base;
    using Microsoft.EntityFrameworkCore;
    public abstract class DbRepositoryBase<TContext, TModel> : IDbRepositoryBase<TModel>
        where TContext : DbContextBase
        where TModel : class
    {
        private readonly TContext _ctx;

        protected DbRepositoryBase(TContext context)
        {
            _ctx = context;
            _ctx.Database.CreateExecutionStrategy();
        }

        protected IQueryable<TModel> Query => _ctx.Set<TModel>().AsNoTracking();

        protected IQueryable<TModel> TrackingQuery => _ctx.Set<TModel>();

        public virtual void Add(TModel entity)
        {
            _ctx.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TModel> entities)
        {
            _ctx.AddRange(entities);
        }

        public void Delete(TModel entity)
        {
            _ctx.Remove(entity);
        }

        protected virtual void DeleteEntities(IEnumerable<TModel> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }
        protected virtual IQueryable<T> GetQuery<T>() where T : class
        {
            // will be removed if EF 3.0 (or will not be virtual)
            // see: https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.0/breaking-changes#qt "Query types are consolidated with entity types"
            return _ctx.Set<T>();
        }

        public Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> TrackingEntity<TEntity>(TEntity e)
            where TEntity : class
        {
            return _ctx.Entry(e);
        }

        public async Task SaveChangesAsync()
        {
            await _ctx.SaveChangesAsync();
        }
        protected virtual void SetValue(TModel entity, object values)
        {
            TrackingEntity(entity).CurrentValues.SetValues(values);

        }
        protected virtual void SetEntityState<TEntity>(TEntity entity, EntityState state)
            where TEntity : class
        {
            TrackingEntity(entity).State = state;
        }


        private void UpdateEntity(TModel entityInDb, TModel entityToDb, Action<TModel, TModel> updateAction = null)
        {
            if (updateAction == null)
            {
                SetValue(entityInDb, entityToDb);
                return;
            }

            updateAction(entityInDb, entityToDb);
        }

        protected void Sync<T>(
         ICollection<T> inDb,
         ICollection<T> toDb,
         Func<T, T, bool> comparer,
         Action<T, T> updateAction = null,
         Action<T>? createAction = null,
         bool skipDelete = false)
        where T : class
        {
            //var modelsName = $"{typeof(T).Name}s";

            inDb ??= new List<T>();
            toDb ??= new List<T>();

            //Log.Trace($"Synchronizing {modelsName}: new {newEntities.Count} / old {entitiesFromDb.Count}");

            foreach (var entityInDb in inDb)
            {
                var entityToDb = toDb.FirstOrDefault(x => comparer(x, entityInDb));
                if (entityToDb == null)
                {
                    if (!skipDelete)
                    {
                        _ctx.Remove(entityInDb!);
                    }
                }
                else
                {
                    updateAction?.Invoke(entityInDb, entityToDb);
                    TrackingEntity(entityInDb).CurrentValues.SetValues(entityToDb);
                }
            }

            var addToDB = new List<T>();
            foreach (var entityToDb in toDb)
            {
                var entityInDb = inDb.FirstOrDefault(x => comparer(x, entityToDb));
                if (entityInDb == null)
                {
                    createAction?.Invoke(entityToDb);
                    addToDB.Add(entityToDb);
                }
            }

            _ctx.AddRange(addToDB);

            //var deleteCnt = entitiesToDelete.Count;
            //var addCnt = entitiesToCreate.Count;
            //var updateCnt = entitiesToUpdate.Count;

            //Log.Trace($"Synchronizer found {deleteCnt} deletes, {addCnt} adds, {updateCnt} updates");


            //Log.Debug($"Synchronized {modelsName}: {deleteCnt} deleted, {addCnt} added, {updateCnt} updated ");
        }

    }
}