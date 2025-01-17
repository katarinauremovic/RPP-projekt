using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public abstract class Repository<T> : IRepository<T>, IDisposable where T : class
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

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await items.FindAsync(id);
        }

        public virtual async Task AddAsync(T item)
        {
            items.Add(item);
            await SaveChangesAsync();
        }

        public virtual async Task RemoveAsync(T item)
        {
            var attachedItem = items.Attach(item);
            items.Remove(attachedItem);
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
