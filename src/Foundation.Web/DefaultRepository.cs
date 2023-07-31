using Attributes.WebAttributes.HttpMethod;
using Attributes.WebAttributes.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Web
{
    [DefaultRepository]
    public class DefaultRepository<T, TKey> where T : class
    {
        private readonly BaseContext<T> _context;

        public DefaultRepository(BaseContext<T> context)
        {
            _context = context;
        }
        [Save]
        public async Task<T> Save(T t)
        {
            var result = _context.Items.Add(t);
            await _context.SaveChangesAsync();
            return result.Entity;
        }
        [Get]
        public async Task<T> Get(TKey id)
        {
            return await _context.Items.FirstAsync();
        }
        [GetAll]
        public async Task<List<T>> GetAll()
        {
            return await _context.Items.ToListAsync();
        }
        [Delete]
        public async Task Delete(T t)
        {
            _context.Items.Remove(t);
            await _context.SaveChangesAsync();
        }
    }
}
