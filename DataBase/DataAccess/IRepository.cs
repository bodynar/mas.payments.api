namespace MAS.Payments.DataBase.Access
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MAS.Payments.Infrastructure.Specification;

    public interface IRepository<TEntity>
        where TEntity : Entity
    {
        Task Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        Task Delete(long id);

        Task<TEntity> Get(long id);

        Task Update(long id, object updatedModel);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> Where(Specification<TEntity> filter);

        Task<bool> Any(Specification<TEntity> predicate);

        Task<int> Count(Specification<TEntity> predicate);
    }
}