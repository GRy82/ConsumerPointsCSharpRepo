using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsumerPoints.Models;
using ConsumerPoints.CustomDataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ConsumerPoints.Data;

namespace ConsumerPoints.Test
{
    [TestClass]
    public class MinHeapTests
    {
        [TestMethod]
        public void Test()
        {
            List<Transaction> transactions = new List<Transaction> {
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=400,
                    Timestamp=new DateTime(2021, 03, 04)
                },
                new Transaction{
                    Payer="Wegman's",
                    Points=300,
                    Timestamp=new DateTime(2021, 05, 04)
                }
            };

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            var firstDequeue = localMemOperations.storedTransactions.Dequeue();

            Assert.AreEqual(firstDequeue, transactions[0]);
            Assert.AreEqual(firstDequeue.Payer, transactions[0].Payer);
            Assert.AreEqual(localMemOperations.storedTransactions.Dequeue(), transactions[1]);
            Assert.IsTrue(localMemOperations.storedTransactions.IsEmpty());
        }

        [TestMethod]
        public void GetPayerBalance_HappyPath_ReturnList()
        {
            List<Transaction> transactions = new List<Transaction> {
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=400,
                    Timestamp=new DateTime(2021, 02, 04)
                },
                new Transaction{
                    Payer="Wegman's",
                    Points=300,
                    Timestamp=new DateTime(2021, 03, 04)
                },
                new Transaction{
                    Payer="CVS Pharmacy",
                    Points=300,
                    Timestamp=new DateTime(2021, 04, 04)
                }
            };

            LocalMemOperations localMemOperations = new LocalMemOperations();
            localMemOperations.AddTransactions(transactions);
            var payerBalances = localMemOperations.GetPayerBalances();
            Assert.AreEqual(localMemOperations.storedTransactions.ToList().Count, 3);
            Assert.AreEqual(payerBalances[0].Points, 700);
            Assert.AreEqual(payerBalances[0].Payer, "CVS Pharmacy");
            Assert.AreEqual(payerBalances[1].Payer, "Wegman's");

        }
    }
}
