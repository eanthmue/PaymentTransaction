using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Common.Infrastructure.Services
{
    public interface ITransactionService
    {
        Task BulkInsert(IList<Transaction> transactions);
        Task<IList<Transaction>> GetByCurrency(string currencyCode);
        Task<IList<Transaction>> GetByDateRange(DateTime fromDate, DateTime toDate);
        Task<IList<Transaction>> GetByStatus(string status);
    }
}
