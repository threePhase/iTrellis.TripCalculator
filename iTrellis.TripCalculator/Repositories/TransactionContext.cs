using iTrellis.TripCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iTrellis.TripCalculator.Repositories
{
    public class TransactionsContext : DbContext 
    {
        public TransactionsContext() : base("TransactionContext")
        {
#if DEBUG
            Database.SetInitializer<TransactionsContext>(new DropCreateIfChangeInitializer());
#else
            // probably don't want to blow away data when we're not in debug mode
            Database.SetInitializer<TransactionsContext>(new CreateIfNotExistsInitializer());
#endif
        }
        public DbSet<Transaction> Transactions { get; set; }

        public void Seed(TransactionsContext Context)
        {
#if DEBUG
            // Add some seed data while debugging
            var transactions = new List<Transaction> {
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
            this.Transactions.AddRange(transactions);
#endif
            // Area to potentially seed production data
            Context.SaveChanges();
        }

        public class DropCreateIfChangeInitializer : DropCreateDatabaseIfModelChanges<TransactionsContext>
        {
            protected override void Seed(TransactionsContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }

        public class CreateIfNotExistsInitializer : CreateDatabaseIfNotExists<TransactionsContext>
        {
            protected override void Seed(TransactionsContext context)
            {
                context.Seed(context);
                base.Seed(context);
            }
        }
    }

}