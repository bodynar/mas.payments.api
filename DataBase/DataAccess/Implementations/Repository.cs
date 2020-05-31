using System;
using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Infrastructure.Extensions;
using MAS.Payments.Infrastructure.Projector;
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

        public void Update(long id, object updatedEntity)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception($"Entity {typeof(TEntity)} with identifier {id} not found");
            }

            DataBaseContext.Entry(entity).CurrentValues.SetValues(updatedEntity); 
        }

        public TEntity Get(long id)
            => (TEntity)DataBaseContext.Set<TEntity>().FirstOrDefault(x => x.Id == id);

        public IQueryable<TEntity> GetAll()
            => DataBaseContext.Set<TEntity>().AsQueryable();

        public IQueryable<TEntity> Where(Specification<TEntity> filter)
            => DataBaseContext.Set<TEntity>().Where(filter);

        public bool Any(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Any(predicate);

        public int Count(Specification<TEntity> predicate)
            => DataBaseContext.Set<TEntity>().Count(predicate);

        public TDestination Get<TDestination>(long id, IProjector<TEntity, TDestination> projector)
            where TDestination : class
        {
            var entity = Get(id);

            if (entity == null)
            {
                throw new Exception($"Entity {typeof(TEntity)} with identifier {id} not found");
            }

            return projector.Project(entity);
        }

        public IEnumerable<TDestination> GetAll<TDestination>(IProjector<TEntity, TDestination> projector)
            where TDestination : class
            => GetAll().Select(projector.Project);

        public IEnumerable<TDestination> Where<TDestination>(Specification<TEntity> filter, IProjector<TEntity, TDestination> projector)
            where TDestination : class
            => Where(filter).Select(projector.Project);
    }
}