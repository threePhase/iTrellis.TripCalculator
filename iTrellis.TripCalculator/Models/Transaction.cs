using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTrellis.TripCalculator.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace iTrellis.TripCalculator.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Owner { get; set; }

        public Transaction() {}

        public Transaction(decimal amount, string payer)
        {
            this.Amount = amount;
            this.Owner = payer;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Attempt to cast
            Transaction transaction = obj as Transaction;
            if ((object)transaction == null)
            {
                return false;
            }

            return this.Id == transaction.Id &&
                   this.Amount == transaction.Amount &&
                   this.Owner == transaction.Owner;
        }

        public override int GetHashCode()
        {
            // simple hash code implementation
            int hash = 13;
            hash = (hash * 7) + this.Id.GetHashCode();
            hash = (hash * 11) + this.Amount.GetHashCode();
            hash = (hash * 13) + this.Owner.GetHashCode();

            return hash;
        }

        public override string ToString()
        {
            return string.Format("{0} - Expense: {1:C}", this.Owner, this.Amount);
        }
    }
}