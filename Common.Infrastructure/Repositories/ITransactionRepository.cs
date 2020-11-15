using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Common.Infrastructure.Repositories
{
    public interface ITransactionRepository<TTransaction> where TTransaction : Transaction
    {
        Task BulkInsert(IList<TTransaction> transactions);
        Task<IList<TTransaction>> GetByCurrency(string currencyCode);
        Task<IList<TTransaction>> GetByDateRange(DateTime fromDate, DateTime toDate);
        Task<IList<TTransaction>> GetByStatus(string status);
    }
}
