using System.Linq;

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
    }
}