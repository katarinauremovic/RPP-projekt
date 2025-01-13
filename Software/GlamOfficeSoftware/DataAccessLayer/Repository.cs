using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public abstract class Repository<T> : IDisposable where T : class
    {
        protected GlamOfficeDbContext context { get; set; }
        protected DbSet<T> items { get; set; }

        protected Repository()
        {
            context = new GlamOfficeDbContext();
            items = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await items.ToListAsync();
        }

        public virtual async Task AddAsync(T item)
        {
            items.Add(item);
            await SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(T item)
        {
            items.Remove(item);
            await SaveChangesAsync();
        }

        public virtual async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
