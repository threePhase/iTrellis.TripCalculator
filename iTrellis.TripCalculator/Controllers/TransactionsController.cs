using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTrellis.TripCalculator.Models;
using iTrellis.TripCalculator.Repositories;

namespace iTrellis.TripCalculator.Controllers
{
    [RoutePrefix("api/transactions")]
    public class TransactionsController : ApiController
    {
        readonly ITransactionRepository repo;

        public TransactionsController()
        {
            this.repo = new TransactionRepository();
        }
        public TransactionsController(ITransactionRepository repo)
        {
            this.repo = repo;
        }

        [Route("")]
        public IEnumerable<Transaction> GetAllTransactions()
        {
            return this.repo.GetAll();
        }

        [Route("{id:int}")]
        public IHttpActionResult GetTransaction(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id must be greater than zero");
            }
            var transaction = this.repo.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [Route("{owner}")]
        public IHttpActionResult GetTransactionsByOwner(string owner)
        {
            if (!string.IsNullOrWhiteSpace(owner))
            {
                return Ok(this.repo.GetByOwner(owner));
            }
            else
            {
                return BadRequest("Owner must not be an empty string");
            }
        }

        [Route("~/api/calculate")]
        public IEnumerable<string> GetSplits()
        {
            return Calculator.DetermineSplits(
                Calculator.CalculateSettlement(this.GetAllTransactions())).Select(s => s.ToString());
        }
    }
}
