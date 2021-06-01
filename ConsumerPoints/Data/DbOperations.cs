using ConsumerPoints.Interfaces;
using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.Data
{
    public class DbOperations : ITransactionStorage
    {
        ApplicationContext _context;
        public DbOperations(ApplicationContext context)
        {
            _context = context;
        }

        public List<PayerPoints> GetPayerBalances()
        {
            Dictionary<string, PayerPoints> balancesByPayer = new Dictionary<string, PayerPoints>();
            var transactions = _context.Transactions.Select(c => new PayerPoints { Payer=c.Payer, Points=c.Points });
            foreach (var transaction in transactions)
                InsertToDictionary(balancesByPayer, transaction.Payer, transaction.Points);

            return balancesByPayer.Values.ToList();
        }
        public void AddTransaction(Transaction transaction)
        {
            _context.Add(transaction);
            _context.SaveChanges();
        }

        public List<PayerPoints> SpendPoints(int withdrawal)
        {

            
            return new List<PayerPoints>();
        }

        private void InsertToDictionary(Dictionary<string, PayerPoints> payerPoints, string Payer, int Points)
        {
            if (payerPoints.ContainsKey(Payer))
                payerPoints[Payer].Points += Points;
            else
                payerPoints.Add(Payer, new PayerPoints
                {
                    Payer = Payer,
                    Points = Points
                });
        }
    }
}
