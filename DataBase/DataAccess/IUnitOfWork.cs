using System;

namespace MAS.Payments.DataBase.Access
{
    public interface IUnitOfWork : IDisposable
    {
        void Rollback();

        bool Commit();
    }
}