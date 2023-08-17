using Codelisk.GeneratorAttributes.WebAttributes.HttpMethod;
using Codelisk.GeneratorAttributes.WebAttributes.Repository;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Web
{
    [DefaultRepository]
    public class DefaultRepository<T, TKey> : IDefaultRepository<T, TKey> where T : class
    {
        private readonly BaseContext _context;

        public DefaultRepository(BaseContext context)
        {
            _context = context;
        }
        [Save]
        public async Task<T> Save(T t)
        {
            var result = _context.Add(t);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        [Get]
        public async Task<T> Get(TKey id)
        {
            return await _context.FindAsync<T>(id);
        }
        [GetAll]
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }
        [Delete]
        public async Task Delete(T t)
        {
            _context.Remove(t);
            await _context.SaveChangesAsync();
        }
    }
}
