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

            Assert.AreEqual(localMemOperations.storedTransactions.Dequeue(), transactions[0]);
            Assert.AreEqual(localMemOperations.storedTransactions.Dequeue(), transactions[1]);
            Assert.IsTrue(localMemOperations.storedTransactions.IsEmpty());
        }
    }
}
