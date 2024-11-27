using System;

namespace PrimoAPITarjetas.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

        public int CardId { get; set; }
        public virtual Card? Card { get; set; }
    }
}
