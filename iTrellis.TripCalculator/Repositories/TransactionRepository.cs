using iTrellis.TripCalculator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace iTrellis.TripCalculator.Repositories
{
    public class TransactionRepository : ITransactionRepository, IDisposable
    {
        private TransactionsContext db = new TransactionsContext();

        public async Task<int> Add(Transaction transaction)
        {
            db.Transactions.Add(transaction);
            return await db.SaveChangesAsync();
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

        public async Task<Transaction> GetById(int id)
        {
            return await db.Transactions.FindAsync(id);
        }

        public IEnumerable<Transaction> GetByOwner(string owner)
        {
            return db.Transactions.Where(t => 
                // have to use ToLower here because using 3 argument string
                // equals is not supported at database
                string.Equals(t.Owner.ToLower(), owner.ToLower())).ToList();
        }

        public async Task<Transaction> Remove(int id)
        {
            var transaction = await GetById(id);
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
            return transaction;
        }

        private bool TransactionExists(int id)
        {
            return db.Transactions.Count(e => e.Id == id) > 0;
        }

        public async Task<bool> Update(Transaction transaction)
        {
            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                return await db.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(transaction.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
    }
}