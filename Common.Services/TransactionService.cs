using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Infrastructure.Services;
using Common.Entities;
using Common.DataAccess.EF.Repositories;
using Common.Services.Helpers;
using System.ComponentModel.DataAnnotations;
using Serilog;
using Newtonsoft.Json;

namespace Common.Services
{
    public class TransactionService : ITransactionService
    {
        private PaymentUnitOfWork paymentUnitOfWork = new PaymentUnitOfWork();
        public TransactionService()
        {
            //Log.Logger = new LoggerConfiguration().WriteTo.WriteTo.File(@"logs\import_log.txt").CreateLogger();
            
        }

        public async Task BulkInsert(IList<Transaction> transactions)
        {
            ICollection<ValidationResult> results;
            bool isValid = true;
            foreach (Transaction entity in transactions)
            {
                if(!EntityValidator.Validate<Transaction>(entity,out results))
                {
                    //string jsonString = JsonSerializer.;
                    Log.Information("Invalid Record:"+entity.ToString());
                    isValid = false;
                }
            }
            if (isValid)
            {
                await this.paymentUnitOfWork.TransactionRepository.BulkInsert(transactions);
            }

        }

        public Task<IList<Transaction>> GetByCurrency(string currencyCode)
        {
            return this.paymentUnitOfWork.TransactionRepository.GetByCurrency(currencyCode);
        }

        public Task<IList<Transaction>> GetByDateRange(DateTime fromDate, DateTime toDate)
        {
            return this.paymentUnitOfWork.TransactionRepository.GetByDateRange(fromDate, toDate);
        }

        public Task<IList<Transaction>> GetByStatus(string status)
        {
            return this.paymentUnitOfWork.TransactionRepository.GetByStatus(status);
        }

        
    }
}
