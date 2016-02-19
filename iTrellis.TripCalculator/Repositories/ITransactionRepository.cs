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
        Task<int> Add(Transaction transaction);
        IEnumerable<Transaction> GetAll();
        Task<Transaction> GetById(int id);
        IEnumerable<Transaction> GetByOwner(string owner);
        Task<Transaction> Remove(int id);
        Task<bool> Update(Transaction transaction);
    }
}
