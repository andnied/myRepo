using Core.Common.Contracts;
using Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Data
{
    public abstract class DataRepositoryBase<TEntity, TContext> : IDataRepository<TEntity>
        where TEntity : class, IIdentifiableEntity, new()
        where TContext : DbContext, new()
    {
        #region PublicMethods

        public TEntity Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var added = AddEntity(context, entity);
                context.SaveChanges();
                return added;
            }
        }

        public IEnumerable<TEntity> Get()
        {
            using (TContext context = new TContext())
            {
                return GetEntities(context);
            }
        }

        public TEntity Get(int id)
        {
            using (TContext context = new TContext())
            {
                return GetEntity(context, id);
            }
        }

        public void Remove(int id)
        {
            using (TContext context = new TContext())
            {
                var entity = GetEntity(context, id);
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public void Remove(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                context.Entry(entity).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var current = UpdateEntity(context, entity);

                SimpleMapper.MapProperties(entity, current);

                context.SaveChanges();
                return current;
            }
        }

        #endregion

        #region AbstractMethods

        protected abstract TEntity AddEntity(TContext context, TEntity entity);
        protected abstract TEntity UpdateEntity(TContext context, TEntity entity);
        protected abstract IEnumerable<TEntity> GetEntities(TContext context);
        protected abstract TEntity GetEntity(TContext context, int id);

        #endregion
    }
}
