namespace MAS.Payments.DataBase.Access
{
    using System;

    public interface IUnitOfWork : IDisposable
    {
        void Rollback();

        bool Commit();
    }
}