using System;
using Microsoft.EntityFrameworkCore.Storage;

namespace MAS.Payments.DataBase.Access
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext _dbContext;

        private IDbContextTransaction _currentTransaction;

        public UnitOfWork(DataBaseContext dbContext)
        {
            _dbContext = dbContext;

            StartTransaction();
        }

        public bool Commit()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Transaction not statred");

            if (_dbContext == null)
                throw new ArgumentException("Bad database context");

            try
            {
                _currentTransaction.Commit();
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                _currentTransaction.Rollback(); // todo update catch block
                return false;
            }
        }

        public void Rollback()
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("Transaction not statred");

            _currentTransaction.Rollback();
        }

        public void Dispose()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
                _currentTransaction = null;
            }
        }

        private void StartTransaction()
        {
            if (_dbContext?.Database == null)
                throw new ArgumentException("Bad database context");

            _currentTransaction = _dbContext.Database.BeginTransaction();
        }
    }
}