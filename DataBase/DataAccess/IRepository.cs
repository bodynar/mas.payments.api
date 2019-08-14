using System.Collections.Generic;
using System.Linq;
using MAS.Payments.Infrastructure.Projector;
using MAS.Payments.Infrastructure.Specification;


namespace MAS.Payments.DataBase.Access
{
    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        void Add(TEntity entity);

        void Update(long id, TEntity updatedEntity);

        void Delete(long id);

        TEntity Get(long id);

        TDestination Get<TDestination>(long id, IProjector<TEntity, TDestination> projector)
            where TDestination: class;

        IQueryable<TEntity> GetAll();

        IEnumerable<TDestination> GetAll<TDestination>(IProjector<TEntity, TDestination> projector)
            where TDestination: class;

        IQueryable<TEntity> Where(Specification<TEntity> filter);

        IEnumerable<TDestination> Where<TDestination>(Specification<TEntity> filter, IProjector<TEntity, TDestination> projector)
            where TDestination: class;

        bool Any(Specification<TEntity> predicate);

        int Count(Specification<TEntity> predicate);
    }
}