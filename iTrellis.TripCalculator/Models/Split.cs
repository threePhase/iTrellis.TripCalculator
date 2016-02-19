using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTrellis.TripCalculator.Models
{
    public class Split
    {
        private decimal _payment;
        public string Creditor { get; set; }
        public string Debitor { get; set; }
        public decimal Payment 
        {
            get { return this._payment; }
            set 
            { 
                if (value > 0)
                {
                    this._payment = value;
                }
                else
                {
                    throw new ArgumentException("Split payments can only be positive in value.");
                }
            }
        }

        public Split(string creditor, string debitor, decimal payment)
        {
            this.Creditor = creditor;
            this.Debitor = debitor;
            this.Payment = payment;
        }

        public Split() { }

        public override int GetHashCode()
        {
            // simple hash code implementation
            int hash = 13;
            hash = (hash * 7) + this.Creditor.GetHashCode();
            hash = (hash * 11) + this.Debitor.GetHashCode();
            hash = (hash * 13) + this.Payment.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Attempt to cast
            Split split = obj as Split;
            if ((object)split == null)
            {
                return false;
            }

            // return true if all fields match
            return this.Payment == split.Payment &&
                   this.Creditor == split.Creditor &&
                   this.Debitor == split.Debitor;
        }

        public override string ToString()
        {
            return string.Format("{0} owes {1} {2:C}", this.Debitor, this.Creditor, this.Payment);
        }
    }
}