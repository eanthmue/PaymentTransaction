using System;
using System.Collections.Generic;
using System.Text;
using Common.Entities;

namespace Common.Infrastructure.Repositories
{
    public interface IPaymentUnitOfWork
    {
        ITransactionRepository<Transaction> TransactionRepository { get; }
        void SaveChangesAsync();
        void Dispose();
    }
}
