using System;
using System.Linq;
using MAS.Payments.Infrastructure.Extensions;
using MAS.Payments.Infrastructure.Specification;

namespace MAS.Payments.DataBase.Access
{
    internal class Repository<TEntity> : IRepository<TEntity>
        where TEntity : Entity
    {
        private DataBaseContext DataBaseContext { get; }

        public Repository(DataBaseContext dataBaseContext)
        {
            DataBaseContext = dataBaseContext;
        }

        public void Add(TEntity entity)
            => DataBaseContext.Set<TEntity>().Add(entity);

        public void Delete(long id)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception($"Entity {typeof(TEntity)} with identifier {id} not found");
            }

            DataBaseContext.Remove(entity);
        }

        public TEntity Get(long id)
            => (TEntity)DataBaseContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);

        public IQueryable<TEntity> GetAll()
            => DataBaseContext.Set<TEntity>().AsQueryable();

        public void Update(long id, TEntity updatedEntity)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception($"Entity {typeof(TEntity)} with identifier {id} not found");
            }

            DataBaseContext.Set<TEntity>().Update(updatedEntity);
        }

        public IQueryable<TEntity> Where(Specification<TEntity> filter)
            => DataBaseContext.Set<TEntity>().Where(filter);

        public bool Any(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Any(predicate);

        public int Count(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Count(predicate);
    }
}