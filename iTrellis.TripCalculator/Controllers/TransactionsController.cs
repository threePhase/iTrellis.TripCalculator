using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using iTrellis.TripCalculator.Models;
using iTrellis.TripCalculator.Repositories;
using System.Web.Http.Description;
using System.Threading.Tasks;

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

        // GET api/transactions
        [Route("")]
        public IEnumerable<Transaction> GetAllTransactions()
        {
            return this.repo.GetAll();
        }

        // GET api/transactions/5
        [Route("{id:int}")]
        [ResponseType(typeof(Transaction))]
        public async Task<IHttpActionResult> GetTransaction(int id)
        {
            Transaction transaction = await this.repo.GetById(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET api/transactions
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

        // PUT api/transactions/5
        [Route("{id:int}")]
        public async Task<IHttpActionResult> PutTransaction(int id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            transaction.Id = id;

            bool updated = await this.repo.Update(transaction);

            if (updated)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            else
            {
                return NotFound();
            }

        }

        // POST api/transactions
        [ResponseType(typeof(Transaction))]
        [Route("")]
        public async Task<IHttpActionResult> PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await this.repo.Add(transaction);

            return CreatedAtRoute("DefaultApi", 
                new { controller = "transactions", id = transaction.Id }, transaction);
        }

        // DELETE api/transactions/5
        [ResponseType(typeof(Transaction))]
        [Route("{id:int}")]
        public async Task<IHttpActionResult> DeleteTransaction(int id)
        {
            var transaction = await this.repo.Remove(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }


        // GET api/calculate
        [Route("~/api/calculate")]
        public IEnumerable<string> GetSplits()
        {
            return Calculator.DetermineSplits(
                Calculator.CalculateSettlement(this.GetAllTransactions())).Select(s => s.ToString());
        }
    }
}
