using ConsumerPoints.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsumerPoints.ServerLogic
{
    public static class ServerHelper
    {
        public static bool TransactionConsumed(int pointsNeeded, Transaction transaction)
        {
            return pointsNeeded > transaction.Points;
        }

        public static void InsertToDictionary(Dictionary<string, PayerPoints> payerPoints, string Payer, int Points)
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
