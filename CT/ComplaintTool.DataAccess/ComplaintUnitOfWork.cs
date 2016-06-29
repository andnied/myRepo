using System;
using System.Collections;
using System.Data.Entity;
using System.Threading;
using ComplaintTool.Common.Config;
using ComplaintTool.Models;
using ComplaintTool.DataAccess.Repos;

namespace ComplaintTool.DataAccess
{
    /// <summary>
    /// Encapsulates Complaint's DbContext, DbContextTransaction and cache Repositories.
    /// </summary>
    public sealed class ComplaintUnitOfWork : IDisposable
    {
        #region Fields

        private ComplaintEntities _context;
        private DbContextTransaction _transaction;
        private bool _disposed;
        private Hashtable _repositories = new Hashtable();
        public bool IsDisposed { get { return _disposed && _context == null; } }
        public bool HasTransaction { get { return _transaction != null; } } 

        #endregion

        #region Constructors

        public ComplaintUnitOfWork(ComplaintEntities context, bool beginTransaction = true)
        {
            _context = context;
            if (beginTransaction)
                _transaction = _context.Database.BeginTransaction();
        } 

        #endregion

        #region Transaction support

        /// <summary>
        /// Begins transaction on Complaint's DbContext. 
        /// </summary>
        public void BeginTransaction()
        {
            if (_disposed)
                throw new ObjectDisposedException("ComplaintDbContext");
            if (_transaction != null)
                throw new InvalidOperationException("Transaction already opened!");
            _transaction = _context.Database.BeginTransaction();
        }
        /// <summary>
        /// Commits transaction on Complaint's DbContext. 
        /// </summary>
        public void Commit()
        {
            if (_disposed)
                throw new ObjectDisposedException("ComplaintDbContext");
            if (_transaction == null)
                throw new InvalidOperationException("Does not have any open transaction!");

            _context.SaveChanges();
            _transaction.Commit();
            _transaction = null;
        } 

        #endregion

        #region Repository sharing

        /// <summary>
        /// Create new instance of Repository or get cached instance.
        /// </summary>
        /// <typeparam name="TRepo">Type of Repository</typeparam>
        /// <returns>instance of Repository</returns>
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

        #region Dispose

        /// <summary>
        /// Dispose Complaint's DbContext, clear cached repositories and optionally rollback transaction if it is not commited. 
        /// </summary>
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

        #region UnitOfWork Factory

        private static ThreadLocal<ComplaintUnitOfWork> _current = new ThreadLocal<ComplaintUnitOfWork>();

        /// <summary>
        /// Always creates new UnitOfWork and open new transaction.
        /// </summary>
        /// <returns>ComplaintUnitOfWork</returns>
        public static ComplaintUnitOfWork Create(bool isInTransaction = true)
        {
            return ComplaintConfig.Instance.Conf != null ? new ComplaintUnitOfWork(new ComplaintEntities(ComplaintConfig.Instance.GetEntityConnectionString()), isInTransaction)
                : new ComplaintUnitOfWork(new ComplaintEntities(), isInTransaction);
        }

        /// <summary>
        /// Get shared per Thread instance of ComplaintUnitOfWork. Create new if current instance not created or instance is already disposed.
        /// </summary>
        /// <returns>ComplaintUnitOfWork</returns>
        public static ComplaintUnitOfWork Get()
        {
            if (_current.Value == null || _current.Value.IsDisposed)
                _current.Value = Create();
            return _current.Value;
        }
        /// <summary>
        /// Dispose shared per Thread instance of ComplaintUnitOfWork.
        /// </summary>
        public static void Clean()
        {
            _current.Value.Dispose();
            _current.Value = null;
        }
        /// <summary>
        /// Dispose all instances of ComplaintUnitOfWork in all running threads.
        /// </summary>
        public static void CleanAll()
        {
            try
            {
                _current.Dispose();
                _current = new ThreadLocal<ComplaintUnitOfWork>();
            }
            catch
            {
                // ignored
            }
        } 

        #endregion
    }
}
