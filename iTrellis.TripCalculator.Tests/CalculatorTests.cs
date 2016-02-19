using System;
using NUnit.Framework;
using Calculator = iTrellis.TripCalculator.Calculator;
using iTrellis.TripCalculator.Models;
using System.Collections.Generic;

namespace iTrellis.TripCalculator.Tests
{
    class CalculatorTests
    {
        List<Transaction> Transactions;

        [SetUp]
        public void Init()
        {
            Transactions = new List<Transaction> {
        	    new Transaction { Id = 1, Amount = 5.75m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = 35, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = 12.79m, Owner = "Louis" },
        	    new Transaction { Id = 4, Amount = 12, Owner = "Carter" },
        	    new Transaction { Id = 5, Amount = 15, Owner = "Carter" },
        	    new Transaction { Id = 6, Amount = 23.23m, Owner = "Carter" },
        	    new Transaction { Id = 7, Amount = 10, Owner = "David" },
        	    new Transaction { Id = 8, Amount = 20, Owner = "David" },
        	    new Transaction { Id = 9, Amount = 38.41m, Owner = "David" },
        	    new Transaction { Id = 10, Amount = 45, Owner = "David" },
        	};
        }

        [Test]
        public void CalculateSettlementThreePersonTripSinglePayer()
        {
            var expected = new Dictionary<string, decimal>
            {
                { "Louis", -18.85m },
                { "Carter", -22.16m },
                { "David", 41.01m }
            };

            IDictionary<string, decimal> actual = 
                Calculator.CalculateSettlement(Transactions);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitThreePersonTripSinglePayer()
        {
            var expected = new List<Split>
            { 
                new Split("David", "Louis", 18.85m), 
                new Split("David", "Carter", 22.16m) 
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(Transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void CalculateSettlementFourPersonTripSinglePayer()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 11, Amount = 9.48m, Owner = "Mitch" },
        	    new Transaction { Id = 12, Amount = 20, Owner = "Mitch" },
        	    new Transaction { Id = 13, Amount = 18.41m, Owner = "Mitch" },
        	    new Transaction { Id = 14, Amount = 15, Owner = "Mitch" },
        	};

            transactions.AddRange(Transactions);

            var expected = new Dictionary<string, decimal>
            {
                { "Louis", -16.47m },
                { "Carter", -19.78m },
                { "David", 43.39m },
                { "Mitch", -7.12m }
            };

            IDictionary<string, decimal> actual = 
                Calculator.CalculateSettlement(transactions);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitFourPersonTripSinglePayer()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 11, Amount = 9.48m, Owner = "Mitch" },
        	    new Transaction { Id = 12, Amount = 20, Owner = "Mitch" },
        	    new Transaction { Id = 13, Amount = 18.41m, Owner = "Mitch" },
        	    new Transaction { Id = 14, Amount = 15, Owner = "Mitch" },
        	};

            transactions.AddRange(Transactions);

            var expected = new List<Split>
            { 
                new Split("David", "Louis", 16.47m), 
                new Split("David", "Carter", 19.78m),
                new Split("David", "Mitch", 7.12m)
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitFourPersonTripEqualPayers()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 1, Amount = 9.48m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = 10.67m, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = 20, Owner = "Carter" },
        	    new Transaction { Id = 4, Amount = 0.15m, Owner = "Carter" },
        	    new Transaction { Id = 5, Amount = 20.15m, Owner = "David" },
        	    new Transaction { Id = 6, Amount = 15, Owner = "Mitch" },
        	    new Transaction { Id = 7, Amount = 5.15m, Owner = "Mitch" },
        	};

            var expected = new List<Split>
            { 
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitThreePersonTripTwoPayers()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 1, Amount = 9.48m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = 10.67m, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = 20, Owner = "Carter" },
        	    new Transaction { Id = 4, Amount = 0.15m, Owner = "Carter" },
        	    new Transaction { Id = 5, Amount = 10.15m, Owner = "David" },
        	};

            var expected = new List<Split>
            { 
                new Split("Louis", "David", 3.33m), 
                new Split("Carter", "David", 3.33m)
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitFourPersonTripTwoPayers()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 1, Amount = 9.48m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = 10.66m, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = 20, Owner = "Carter" },
        	    new Transaction { Id = 4, Amount = 0.16m, Owner = "Carter" },
        	    new Transaction { Id = 5, Amount = 10.15m, Owner = "David" },
        	    new Transaction { Id = 6, Amount = 10.12m, Owner = "Mitch" },
        	};

            var expected = new List<Split>
            { 
                new Split("Louis", "David", 4.99m),
                new Split("Louis", "Mitch", 0.01m), 
                new Split("Carter", "Mitch", 5.01m), 
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void DetermineSplitFourPersonTripThreePayers()
        {
            var transactions = new List<Transaction> {
        	    new Transaction { Id = 1, Amount = 9.48m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = 10.66m, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = 20, Owner = "Carter" },
        	    new Transaction { Id = 4, Amount = 0.16m, Owner = "Carter" },
        	    new Transaction { Id = 5, Amount = 10.15m, Owner = "David" },
        	    new Transaction { Id = 6, Amount = 18.12m, Owner = "Mitch" },
        	};

            var expected = new List<Split>
            { 
                new Split("Louis", "David", 3.00m), 
                new Split("Carter", "David", 3.01m),
                new Split("Mitch", "David", 0.98m),
            };

            IDictionary<string, decimal> settlement = 
                Calculator.CalculateSettlement(transactions);

            IEnumerable<Split> actual = 
                Calculator.DetermineSplits(settlement);
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
