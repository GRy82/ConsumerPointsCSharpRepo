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


        // The following two static methods are used in different circumstances depending on the type of the dictionary. 
        // They each serve the same purpose, but with different logic which corresponds to the dictionary type used.

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

        public static void InsertToDictionary(Dictionary<string, int> payerPoints, string payer, int points)
        {
            if (payerPoints.ContainsKey(payer))
                payerPoints[payer] += points;
            else
                payerPoints.Add(payer, points);
        }
    }
}
