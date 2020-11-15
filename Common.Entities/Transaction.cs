using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Entities
{
    public class Transaction : BaseEntity
    {
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public string Status { get; set; }

    }
}
