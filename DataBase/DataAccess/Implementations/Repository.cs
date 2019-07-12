using System;
using System.Linq;

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
            => DataBaseContext.Add(entity);

        public void Delete(long id)
        {
            var entity = Get(id);
            DataBaseContext.Remove(entity);
        }

        public TEntity Get(long id)
            => (TEntity)DataBaseContext.Find(typeof(TEntity), id);

        public IQueryable<TEntity> GetAll()
            => DataBaseContext.Set<TEntity>().AsQueryable();

        public void Update(long id, TEntity updatedEntity)
        {
            var entity = Get(id);
            if (entity == null)
            {
                throw new Exception($"Entity {typeof(TEntity)} with identifier {id} not found");
            }

            DataBaseContext.Update(updatedEntity);
        }
    }
}