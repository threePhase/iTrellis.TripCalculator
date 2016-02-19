using System;
using System.Linq;
using NUnit.Framework;
using iTrellis.TripCalculator.Models;
using iTrellis.TripCalculator.Controllers;
using iTrellis.TripCalculator.Repositories.Fakes;
using System.Web.Http.Results;
using System.Collections.Generic;

namespace iTrellis.TripCalculator.Tests
{
    class TransactionsControllerTests
    {
        Transaction[] Transactions;

        [SetUp]
        public void Init()
        {
            Transactions = new Transaction[] {
        	    new Transaction { Id = 1, Amount = -5.75m, Owner = "Louis" },
        	    new Transaction { Id = 2, Amount = -35, Owner = "Louis" },
        	    new Transaction { Id = 3, Amount = -12.79m, Owner = "Louis" },
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
        public void GetAllTransactions()
        {
            var repo = new StubITransactionRepository
            {
                GetAll = () => Transactions
            };

            var controller = new TransactionsController(repo);
            var actual = controller.GetAllTransactions();
            CollectionAssert.AreEquivalent(Transactions, actual);
        }

        [Test]
        public void GetTransactionById()
        {
            var expected = Transactions[1];
            var repo = new StubITransactionRepository
            {
                GetByIdInt32 = id => expected
            };

            var controller = new TransactionsController(repo);
            var actual = controller.GetTransaction(2);

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Transaction>), actual);
            // Cast to the appropriate type
            OkNegotiatedContentResult<Transaction> response = actual as OkNegotiatedContentResult<Transaction>;
            Assert.AreEqual(expected, response.Content);
        }

        [Test]
        public void GetTransactionsByOwner()
        {
            var expected = Transactions.Where(t => t.Owner == "David");
            var repo = new StubITransactionRepository
            {
                GetByOwnerString = owner => expected
            };

            var controller = new TransactionsController(repo);
            var actual = controller.GetTransactionsByOwner("Carter");

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<Transaction>>), actual);
            // Cast to the appropriate type
            OkNegotiatedContentResult<IEnumerable<Transaction>> response = 
                actual as OkNegotiatedContentResult<IEnumerable<Transaction>>;
            CollectionAssert.AreEquivalent(expected, response.Content);
        }
    }
}
