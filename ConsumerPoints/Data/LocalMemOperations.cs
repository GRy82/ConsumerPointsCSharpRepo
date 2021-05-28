using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;

namespace ConsumerPoints.Data
{
    public class LocalMemOperations : ITransactionStorage
    {

        public static List<Transaction> transactions = new List<Transaction>() {

            new Transaction { Payer="CVS Pharmacy", Points=200, Timestamp=new DateTime(2021, 01, 15) },
            new Transaction { Payer="CVS Pharmacy", Points=400, Timestamp=new DateTime(2020, 12, 25) }
        };

        public List<PayerBalance> GetPayerBalances() {
            return new List<PayerBalance>();
        }

        public void AddTransactions(List<Transaction> transactions)
        {

        }

        public List<ExpenditureByPayer> SpendPoints(int pointToBeSpent)
        {
            return new List<ExpenditureByPayer>();
        }
    }
}
