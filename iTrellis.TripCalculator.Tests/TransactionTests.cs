using System;
using NUnit.Framework;
using iTrellis.TripCalculator.Models;

namespace iTrellis.TripCalculator.Tests
{
    [TestFixture]
    class TransactionTests
    {
        Transaction Transaction;
        [SetUp]
        public void Init()
        {
            Transaction = new Transaction { Id = 1, Amount = 16.47m, Owner = "Louis" };
        }
        
        [Test]
        public void Equals()
        {
            var actual = new Transaction(16.47m,  "Louis"); 
            actual.Id = 1;
            var expected = Transaction;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetHashCode()
        {
            var actual = new Transaction(15.13m, "Carter" ).GetHashCode();
            var expected = Transaction;
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void ToString()
        {
            var expense = new Transaction(5.75m, "Louis");
            var expected = "Louis - Expense: $5.75";
            Assert.AreEqual(expected, expense.ToString());
        }
    }
}
