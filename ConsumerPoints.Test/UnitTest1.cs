using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConsumerPoints.Models;
using ConsumerPoints.CustomDataStructures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ConsumerPoints.Test
{
    [TestClass]
    public class MinHeapTests
    {
        [TestMethod]
        public void Insert_WhenEmpty_ToZeroIndex()
        {
            //Arrange
            MinHeap minHeap = new MinHeap();
            minHeap.Insert(new Transaction
            {
                Payer="CVS Pharmacy",
                Points=400,
                Timestamp=DateTime.Now()
            });
            //Act
            //Assert
        }
    }
}
