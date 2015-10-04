using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using APIfy.Repositories.Exceptions;

namespace APIfy.Repositories
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork(DbContext db)
        {
            _db = db;
        }

        public void BeginTransaction()
        {
            _dbContextTransaction = _db.Database.BeginTransaction();
        }

        public int CommitTransaction()
        {
            int result = SaveChanges();
            _dbContextTransaction.Commit();
            return result;
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }

        public int SaveChanges()
        {
            try
            {
                return _db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
        }

        public Task<int> SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }
       
        protected virtual void Dispose(bool disposing)
        {
            _db.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private DbContext _db;
        private DbContextTransaction _dbContextTransaction;
    }
}
