using System.Linq;
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

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Where(Specification<TEntity> filter);

        bool Any(Specification<TEntity> predicate);

        int Count(Specification<TEntity> predicate);
    }
}