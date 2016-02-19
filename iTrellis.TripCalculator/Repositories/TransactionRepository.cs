using iTrellis.TripCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iTrellis.TripCalculator.Repositories
{
    public class TransactionRepository : ITransactionRepository, IDisposable
    {
        private TransactionsContext db = new TransactionsContext();

        public void Add(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            db.SaveChanges();
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return db.Transactions;
        }

        public Transaction GetById(int id)
        {
            return db.Transactions.FirstOrDefault(t => t.Id == id);
        }

        public IEnumerable<Transaction> GetByOwner(string owner)
        {
            return db.Transactions.Where(t => 
                // have to use ToLower here because using 3 argument string
                // equals is not supported at database
                string.Equals(t.Owner.ToLower(), owner.ToLower())).ToList();
        }
    }
}