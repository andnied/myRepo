using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaMvp.DataAccess.Repos
{
    public abstract class RepositoryBase
    {
        private readonly DbContext _context;

        protected RepositoryBase(DbContext context)
        {
            if (context == null) throw new ArgumentNullException("context");
            _context = context;
        }

        protected void Update<T>(T entity) where T : class
        {
            GetDbSet<T>().Attach(entity);
            SetEntityState(entity, EntityState.Modified);
        }

        protected T Add<T>(T entity) where T : class
        {
            var added = GetDbSet<T>().Add(entity);
            SetEntityState(entity, EntityState.Added);
            return added;
        }

        protected void SetEntityState(object entity, EntityState entityState)
        {
            _context.Entry(entity).State = entityState;
        }

        protected DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            return _context.Set<TEntity>();
        }

        protected TContext GetContext<TContext>() where TContext : DbContext
        {
            return (TContext)_context;
        }
    }
}
