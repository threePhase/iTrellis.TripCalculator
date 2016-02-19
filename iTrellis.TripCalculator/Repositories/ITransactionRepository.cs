using iTrellis.TripCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iTrellis.TripCalculator.Repositories
{
    public interface ITransactionRepository
    {
        void Add(Transaction transaction);
        IEnumerable<Transaction> GetAll();
        Transaction GetById(int id);
        IEnumerable<Transaction> GetByOwner(string owner);
    }
}
