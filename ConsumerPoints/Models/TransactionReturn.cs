using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Models
{
    public class TransactionReturn
    {
        public string Payer { get; set; }

        public int Points { get; set; }

        public DateTime Timestamp { get; set; }

        public static TransactionReturn TransactionConvert(Transaction transaction)
        {
            return new TransactionReturn
            {
                Payer = transaction.Payer,
                Points = transaction.Points,
                Timestamp = transaction.Timestamp
            });
        }
    }

}
