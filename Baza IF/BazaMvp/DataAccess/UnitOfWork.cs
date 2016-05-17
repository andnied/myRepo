using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity;
using System.Collections;
using BazaMvp.DataAccess.Repos;

namespace BazaMvp.DataAccess
{
    public class UnitOfWork : IDisposable
    {
        private static ThreadLocal<UnitOfWork> Current = new ThreadLocal<UnitOfWork>();
        private IFModel _context;
        private DbContextTransaction _transaction;
        private bool _disposed;
        private Hashtable _repositories = new Hashtable();

        public bool IsDisposed { get { return _disposed && _context == null; } }
        public bool HasTransaction { get { return _transaction != null; } }

        private UnitOfWork(IFModel context)
        {
            _context = context;
            _transaction = _context.Database.BeginTransaction();
        }

        #region Transaction

        public void BeginTransaction()
        {
            if (_disposed)
                throw new ObjectDisposedException("ComplaintDbContext");
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already open!");
            _transaction = _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException("ComplaintDbContext");
            if (_transaction == null)
                throw new InvalidOperationException("Could not detect an open transaction!");

            _context.SaveChanges();
            _transaction.Commit();
            _transaction = null;
        }

        #endregion

        #region Repos

        public TRepo Repo<TRepo>() where TRepo : RepositoryBase
        {
            if (_disposed)
                throw new ObjectDisposedException("ComplaintDbContext");

            var typeName = typeof(TRepo).FullName;
            if (_repositories.ContainsKey(typeName))
                return (TRepo)_repositories[typeName];

            try
            {
                var repo = Activator.CreateInstance(typeof(TRepo), _context);
                _repositories.Add(typeName, repo);
                return (TRepo)repo;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Repository {0} can't be created!", typeName), ex);
            }
        }

        #endregion

        #region Factory

        public static UnitOfWork Create()
        {
            return new UnitOfWork(new IFModel());
        }

        public static void Clean()
        {
            Current.Value.Dispose();
            Current.Value = null;
        }

        public static void CleanAll()
        {
            try
            {
                Current.Dispose();
                Current = new ThreadLocal<UnitOfWork>();
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Dispose

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    _repositories.Clear();
                    _repositories = null;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    if (_transaction != null)
                        _transaction.Rollback();
                    _transaction = null;
                }
                catch
                {
                    // ignored
                }

                try
                {
                    _context.Dispose();
                    _context = null;
                }
                catch
                {
                    // ignored
                }

                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
