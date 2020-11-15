using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;

namespace Common.DataAccess.EF
{
    public class PaymentContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

        public PaymentContext() : base("name=PaymentContext")
        {
            InitContextSettings();
        }

        public PaymentContext(string connectionString) : base(connectionString)
        {
            InitContextSettings();
        }

        protected void InitContextSettings()
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }
    }
}
