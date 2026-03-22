namespace MAS.Payments.DataBase.Access
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Extensions;
    using MAS.Payments.Infrastructure.Specification;

    using Microsoft.EntityFrameworkCore;

    internal class Repository<TEntity>(
        DataBaseContext dataBaseContext
    ) : IRepository<TEntity>
        where TEntity : Entity
    {
        private DataBaseContext DataBaseContext { get; } = dataBaseContext;

        public async Task Add(TEntity entity)
            => await DataBaseContext.Set<TEntity>().AddAsync(entity);

        public async Task AddRange(IEnumerable<TEntity> entities)
            => await DataBaseContext.Set<TEntity>().AddRangeAsync(entities);

        public async Task Delete(Guid id)
        {
            var entity = await Get(id)
                ?? throw new EntityNotFoundException(typeof(TEntity), id);

            DataBaseContext.Remove(entity);
        }

        public Task DeleteRange(IEnumerable<TEntity> entities)
        {
            DataBaseContext.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task Update(Guid id, object updatedModel)
        {
            var entity = await Get(id)
                ?? throw new EntityNotFoundException(typeof(TEntity), id);

            DataBaseContext.Entry(entity).CurrentValues.SetValues(updatedModel); 
        }

        public async Task<TEntity> Get(Guid id)
            => await DataBaseContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);

        public IQueryable<TEntity> GetAll()
            => DataBaseContext.Set<TEntity>().AsQueryable();

        public IQueryable<TEntity> Where(Specification<TEntity> filter)
            => DataBaseContext.Set<TEntity>().Where(filter);

        public Task<bool> Any(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Any(predicate);

        public Task<int> Count(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Count(predicate);
    }
}
