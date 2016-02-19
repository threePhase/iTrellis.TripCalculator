using System;
using NUnit.Framework;
using iTrellis.TripCalculator.Models;

namespace iTrellis.TripCalculator.Tests
{
    class SplitTests
    {
        Split Split;
        [SetUp]
        public void Init()
        {
            Split = new Split { Creditor = "David", Debitor = "Louis", Payment = 16.47m };
        }

        [Test]
        public void ConstructorValidParameters()
        {
            decimal actual = new Split("David", "Louis", 16.47m).Payment;
            decimal expected = Split.Payment;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ConstructorNegativePaymentThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>( 
                delegate {
                    new Split("David", "Louis", -16.47m);
                }
            );
        }

        [Test]
        public void Equals()
        {
            Split actual = new Split("David", "Louis", 16.47m);
            Split expected = Split;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetHashCode()
        {
            int actual = new Split("David", "Louis", 6.71m).GetHashCode();
            int expected = Split.GetHashCode();
            Assert.AreNotEqual(expected, actual);
        }

        [Test]
        public void ToString()
        {
            string expected = "Louis owes David $16.47";
            string actual = new Split("David", "Louis", 16.47m).ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
