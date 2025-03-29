namespace MAS.Payments.DataBase.Access
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Infrastructure.Exceptions;
    using MAS.Payments.Infrastructure.Extensions;
    using MAS.Payments.Infrastructure.Specification;

    internal class Repository<TEntity>(DataBaseContext dataBaseContext) : IRepository<TEntity>
        where TEntity : Entity
    {
        private DataBaseContext DataBaseContext { get; } = dataBaseContext;

        public void Add(TEntity entity)
            => DataBaseContext.Set<TEntity>().Add(entity);

        public void AddRange(IEnumerable<TEntity> entities)
            => DataBaseContext.Set<TEntity>().AddRange(entities);

        public void Delete(long id)
        {
            var entity = Get(id) ?? throw new EntityNotFoundException(typeof(TEntity), id);

            DataBaseContext.Remove(entity);
        }

        public void Update(long id, object updatedModel)
        {
            var entity = Get(id) ?? throw new EntityNotFoundException(typeof(TEntity), id);

            DataBaseContext.Entry(entity).CurrentValues.SetValues(updatedModel); 
        }

        public TEntity Get(long id)
            => DataBaseContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);

        public IQueryable<TEntity> GetAll()
            => DataBaseContext.Set<TEntity>().AsQueryable();

        public IQueryable<TEntity> Where(Specification<TEntity> filter)
            => DataBaseContext.Set<TEntity>().Where(filter);

        public bool Any(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Any(predicate);

        public int Count(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Count(predicate);
    }
}