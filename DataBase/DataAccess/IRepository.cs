namespace MAS.Payments.DataBase.Access
{
    using System.Collections.Generic;
    using System.Linq;

    using MAS.Payments.Infrastructure.Specification;

    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(long id);

        TEntity Get(long id);

        void Update(long id, object updatedModel);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Where(Specification<TEntity> filter);

        bool Any(Specification<TEntity> predicate);

        int Count(Specification<TEntity> predicate);
    }
}