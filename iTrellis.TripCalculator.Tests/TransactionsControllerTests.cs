using System;
using System.Linq;
using NUnit.Framework;
using iTrellis.TripCalculator.Models;
using iTrellis.TripCalculator.Controllers;
using iTrellis.TripCalculator.Repositories.Fakes;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public void GetAllTransactionsReturnsEverything()
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
        public async Task GetTransactionReturnsCorrectItem()
        {
            var expected = Transactions[1];
            var repo = new StubITransactionRepository
            {
                GetByIdInt32 = id => Task.FromResult(expected)
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.GetTransaction(2);

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Transaction>), actual);
            // Cast to the appropriate type
            OkNegotiatedContentResult<Transaction> response = actual as OkNegotiatedContentResult<Transaction>;
            Assert.That(actual, Is.EqualTo(response));
        }

        [Test]
        public async Task GetTransactionReturnsNotFoundWhenRepoReturnsNull()
        {
            Transaction expected = null;
            var repo = new StubITransactionRepository
            {
                GetByIdInt32 = id => Task.FromResult(expected)
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.GetTransaction(2);

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(NotFoundResult), actual);
            // Cast to the appropriate type
            NotFoundResult response = actual as NotFoundResult;
            Assert.That(actual, Is.EqualTo(response));
        }

        [Test]
        public void GetTransactionsByOwnerReturnsSingleOwnerTransactions()
        {
            var expected = Transactions.Where(t => t.Owner == "David");
            var repo = new StubITransactionRepository
            {
                GetByOwnerString = owner => expected
            };

            var controller = new TransactionsController(repo);
            var actual = controller.GetTransactionsByOwner("David");

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<IEnumerable<Transaction>>), actual);
            // Cast to the appropriate type
            OkNegotiatedContentResult<IEnumerable<Transaction>> response = 
                actual as OkNegotiatedContentResult<IEnumerable<Transaction>>;
            CollectionAssert.AreEquivalent(expected, response.Content);
        }

        [Test]
        public async Task PutTransactionUpdatesRepository()
        {
            bool wasCalled = false;
            var repo = new StubITransactionRepository
            {
                UpdateTransaction = t => Task.FromResult(wasCalled = true)
            };

            var controller = new TransactionsController(repo);
            var transaction = new Transaction { Id = 2 };
            var actual = await controller.PutTransaction(2, transaction);

            // Check we got the appropriate response
            Assert.True(wasCalled);
        }

        [Test]
        public async Task PutTransactionReturnsNotFoundWhenRepositoryReturnsFalse()
        {
            bool wasCalled = false;
            var repo = new StubITransactionRepository
            {
                UpdateTransaction = transaction => Task.FromResult(false)
            };
            
            var controller = new TransactionsController(repo);
            var actual = await controller.PutTransaction(2, new Transaction());

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(NotFoundResult), actual);
            // Cast to the appropriate type
            NotFoundResult response = actual as NotFoundResult;
            Assert.That(actual, Is.EqualTo(response));
        }

        [Test]
        public async Task PutTransactionSetsIdBeforeUpdatingRepository()
        {
            int updatedId = 0;
            var repo = new StubITransactionRepository
            {
                UpdateTransaction = t => { updatedId = t.Id; return Task.FromResult(true); }
            };

            var controller = new TransactionsController(repo);
            var transaction = new Transaction { Id = 0 };
            var actual = await controller.PutTransaction(2, transaction);

            Assert.That(2, Is.EqualTo(updatedId));
        }

        [Test]
        public async Task DeleteTransactionCallsRepositoryRemove()
        {
            int removedId = 0;
            var transaction = Transactions[1];
            var repo = new StubITransactionRepository
            {
                RemoveInt32 = id => { removedId = id; return Task.FromResult(transaction); }
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.DeleteTransaction(2);

            Assert.That(2, Is.EqualTo(removedId));
        }

        [Test]
        public async Task DeleteTransactionReturnsDeletedTransaction()
        {
            var expected = Transactions[1];
            var repo = new StubITransactionRepository
            {
                RemoveInt32 = id => Task.FromResult(expected)
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.DeleteTransaction(2);

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(OkNegotiatedContentResult<Transaction>), actual);
            // Cast to the appropriate type
            OkNegotiatedContentResult<Transaction> response = actual as OkNegotiatedContentResult<Transaction>;
            Assert.That(expected, Is.EqualTo(response.Content));
        }

        [Test]
        public async Task PostTransactionReturnsCreatedStatusCode()
        {
            var repo = new StubITransactionRepository
            {
                AddTransaction = transaction => Task.FromResult(transaction.Id)
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.PostTransaction(new Transaction{ Id = 2 });

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(CreatedAtRouteNegotiatedContentResult<Transaction>), actual);
        }

        [Test]
        public async Task PostTransactionCallsRepositoryAdd()
        {
            Transaction expected = null;
            var repo = new StubITransactionRepository
            {
                AddTransaction = transaction => 
                { 
                    expected = transaction; 
                    return Task.FromResult(transaction.Id); 
                }
            };

            var controller = new TransactionsController(repo);
            var actual = await controller.PostTransaction(new Transaction{ Id = 2 });

            // Check we got the appropriate response
            Assert.IsInstanceOf(typeof(CreatedAtRouteNegotiatedContentResult<Transaction>), actual);
            CreatedAtRouteNegotiatedContentResult<Transaction> response = 
                actual as CreatedAtRouteNegotiatedContentResult<Transaction>;

            Assert.That(expected, Is.EqualTo(response.Content));
        }
    }
}