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

        #region Constructors

        protected DataRepositoryBase()
        {
            _context = new TContext();
        }

        protected DataRepositoryBase(TContext context)
        {
            _context = context;
        }

        #endregion

        #region PublicMethods

        public TEntity Add(TEntity entity)
        {
            var added = AddEntity(_context, entity);
            _context.SaveChanges();
            return added;
        }

        public IEnumerable<TEntity> Get()
        {
            return GetEntities(_context);
        }

        public TEntity Get(int id)
        {
            return GetEntity(_context, id);
        }

        public void Remove(int id)
        {
            var entity = GetEntity(_context, id);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            _context.SaveChanges();
        }

        public TEntity Update(TEntity entity)
        {
            var current = UpdateEntity(_context, entity);

            SimpleMapper.MapProperties(entity, current);

            _context.SaveChanges();
            return current;
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
