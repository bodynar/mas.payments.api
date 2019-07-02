using System.Linq;
using System;

namespace MAS.Payments.DataBase.Access
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);

        void Update(Guid id, TEntity updatedEntity);

        void Delete(Guid id);

        TEntity Get(Guid id);

        IQueryable<TEntity> GetAll();
    }
}