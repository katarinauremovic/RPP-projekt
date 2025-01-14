using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public abstract class Service<TEntity> where TEntity : class
    {
        protected readonly IRepository<TEntity> _repository;

        protected Service(IRepository<TEntity> repository)
        {
            _repository = repository;
        }

        public virtual Task<TEntity> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public virtual Task AddAsync(TEntity entity)
        {
            return _repository.AddAsync(entity);
        }

        public virtual Task RemoveAsync(TEntity entity)
        {
            return _repository.RemoveAsync(entity);
        }
    }
}
