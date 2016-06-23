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
        protected readonly TContext _context;

        protected DataRepositoryBase()
        {
            _context = new TContext();
        }

        protected DataRepositoryBase(TContext context)
        {
            _context = context;
        }

        #region PublicMethods

        public TEntity Add(TEntity entity)
        {
            //using (TContext context = new TContext())
            //{
            var added = AddEntity(_context, entity);
            _context.SaveChanges();
            return added;
            //}
        }

        public IEnumerable<TEntity> Get()
        {
            //using (TContext context = new TContext())
            //{
            return GetEntities(_context);
            //}
        }

        public TEntity Get(int id)
        {
            //using (TContext context = new TContext())
            //{
            return GetEntity(_context, id);
            //}
        }

        public void Remove(int id)
        {
            //using (TContext context = new TContext())
            //{
            var entity = GetEntity(_context, id);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
            //}
        }

        public void Remove(TEntity entity)
        {
            //using (TContext context = new TContext())
            //{
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
            //}
        }

        public TEntity Update(TEntity entity)
        {
            //using (TContext context = new TContext())
            //{
            var current = UpdateEntity(_context, entity);

            SimpleMapper.MapProperties(entity, current);

            _context.SaveChanges();
            return current;
            //}
        }

        public void Dispose()
        {
            try
            {
                _context.Dispose();

                GC.SuppressFinalize(this);
            }
            catch { }
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
