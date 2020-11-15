using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Common.Infrastructure.Repositories;

namespace Common.DataAccess.EF.Repositories
{
    public class TransactionRepository : BaseRepository<Transaction>, ITransactionRepository<Transaction>
    {
        internal PaymentContext context;
        internal DbSet<Transaction> dbSet;

        public TransactionRepository(PaymentContext context)
        {
            this.context = context;
            this.dbSet = context.Set<Transaction>();
        }

        public async Task BulkInsert(IList<Transaction> transactions)
        {
            await this.dbSet.BulkInsertAsync(transactions);
        }

        public async Task<IList<Transaction>> GetByCurrency(string currencyCode)
        {
            return await dbSet.Where(x => x.CurrencyCode == currencyCode).ToListAsync();
        }

        public async Task<IList<Transaction>> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            toDate = toDate.AddHours(12);

            return await dbSet.Where(x => x.TransactionDate >= fromDate && x.TransactionDate < toDate).ToListAsync();
        }

        public async Task<IList<Transaction>> GetByStatus(string status)
        {
            return await dbSet.Where(x => x.Status == status).ToListAsync();
        }
    }
}
