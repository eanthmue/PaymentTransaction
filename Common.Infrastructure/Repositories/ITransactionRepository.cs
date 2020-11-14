using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Infrastructure.Repositories
{
    public interface ITransactionRepository<TTransaction> where TTransaction : Transaction
    {
        Task AddRange(IList<TTransaction> transactions);
        Task<IList<TTransaction>> GetByCurrency(string currencyCode);
        Task<IList<TTransaction>> GetByDateRange(DateTime fromDate, DateTime toDate);
        Task<IList<TTransaction>> GetByStatus(string status);
    }
}
