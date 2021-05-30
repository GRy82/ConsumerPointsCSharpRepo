using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsumerPoints.Models;

namespace ConsumerPoints.CustomDataStructures
{
    public class MinHeap
    {

        private int capacity = 10;
        private int count = 0;
        private Transaction[] transactions = new Transaction[10];


        public void Insert(Transaction transaction)
        {
            if (IsFull()) Resize();

            int addedindex = count++;
            transactions[addedindex] = transaction;
       
            BubbleUp(transaction, addedindex);
        }

        public Transaction Remove()
        {
            if (IsEmpty()) throw new Exception("Heap is Empty. There is no transaction to remove.");

            Transaction removal = transactions[0];
            transactions[0] = transactions[count - 1];
            BubbleDown(transactions[0], 0);

            return removal;
        }

        private void BubbleUp(Transaction transaction, int transactionIndex)
        {
            int parentIndex = transactionIndex % 2 == 0 ? 
                transactionIndex / 2 - 1 : transactionIndex / 2;

            if (transactionIndex == 0 || transaction.Timestamp.CompareTo(transactions[parentIndex]) > 0) 
                return;

            transactions[transactionIndex] = transactions[parentIndex];
            transactions[parentIndex] = transaction;

            BubbleUp(transaction, parentIndex);
        }

        private void BubbleDown(Transaction transaction, int parentIndex)
        {
            if (!HasLeftChild(parentIndex)) return;

            int leftChildIndex = parentIndex * 2 + 1;
            int rightChildIndex = parentIndex * 2 + 2;
            int childToCompareIndex = leftChildIndex;

            if (HasLeftChild(parentIndex) && HasRightChild(parentIndex))
                childToCompareIndex = transactions[leftChildIndex].Timestamp.CompareTo(transactions[rightChildIndex].Timestamp) > 0 ?
                    rightChildIndex : leftChildIndex;

            if (transaction.Timestamp.CompareTo(transactions[childToCompareIndex]) < 0)
                return;

            transactions[parentIndex] = transactions[childToCompareIndex];
            transactions[childToCompareIndex] = transaction;
            BubbleUp(transaction, childToCompareIndex);
        }

        private bool IsFull()
        {
            if (count == capacity) return true;

            return false;
        }

        private bool IsEmpty()
        {
            if (count == 0) return true;

            return false;
        }

        private void Resize()
        {
            capacity *= 2;
            Transaction[] temp = transactions;
            transactions = new Transaction[capacity];
            for (int i = 0; i < temp.Length; i++)
                transactions[i] = temp[i];
        }

        private bool HasLeftChild(int index)
        {
            return index * 2 + 1 < count;
        }

        private bool HasRightChild(int index)
        {
            return index * 2 + 2 < count;
        }
    }
}
