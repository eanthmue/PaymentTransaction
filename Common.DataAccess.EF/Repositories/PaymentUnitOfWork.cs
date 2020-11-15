using Common.Entities;
using Common.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF.Repositories
{
    public class PaymentUnitOfWork : IPaymentUnitOfWork
    {
        private PaymentContext context;
        

        public ITransactionRepository<Transaction> TransactionRepository => new TransactionRepository(context);

        public PaymentUnitOfWork()
        {
            context = new PaymentContext();
        }

        public PaymentUnitOfWork(string connectionString)
        {
            context = new PaymentContext(connectionString);
        }
       
        public void SaveChangesAsync()
        {
            context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
