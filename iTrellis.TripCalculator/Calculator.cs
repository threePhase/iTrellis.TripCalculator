using iTrellis.TripCalculator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iTrellis.TripCalculator
{
    public static class Calculator
    {
        /// <summary>
        /// Calculate amounts owed by all parties. In the case that the
        /// split causes a fraction of a penny, the individual who owes the
        /// least will be rounded up a cent, and the individual who owes the
        /// most will be rounded down a cent.
        /// </summary>
        /// <param name="transactions">Transactions to be settled</param>
        /// <returns>
        /// Dictionary of results keyed by each distinct Owner in transactions
        /// with their corresponding amounts owed.
        /// </returns>
        public static IDictionary<string, decimal> CalculateSettlement(IEnumerable<Transaction> transactions) 
        {
            var settlement = new Dictionary<string, decimal>();
            decimal total = 0;
            decimal ownersCount = 0;
            // sum up transactions paid by each individual
            foreach (var transaction in transactions)
            {
                // total value of the trip includes both credits and debits
                total += transaction.Amount;
                if (settlement.ContainsKey(transaction.Owner))
                {
                    // sum with previous transactions
                    settlement[transaction.Owner] += transaction.Amount;
                }
                else
                {
                    // new individual is being added to calculation
                    ownersCount++;
                    settlement[transaction.Owner] = transaction.Amount;
                }
            }

            var owners = new List<string>(settlement.Keys);
            decimal individualResponsibility = total / ownersCount;
            decimal roundedResponsibility =
                // multiply by 100 before ceiling to round up cents only
                decimal.Ceiling(individualResponsibility * 100) / 100;

            bool roundedCent = individualResponsibility - roundedResponsibility != 0;
            if (roundedCent)
            {
                // most individuals will fare better on the extra cent
                individualResponsibility = roundedResponsibility - 0.01m;
            }

            string largestPayer = null;
            decimal greatestPaid = 0;
            foreach (string owner in owners)
            {
                var settlementValue = settlement[owner];
                decimal newSettlementValue = settlementValue - individualResponsibility;
                settlement[owner] = newSettlementValue;

                if (newSettlementValue > greatestPaid)
                {
                    greatestPaid = newSettlementValue;
                    largestPayer = owner;
                } 
            }

            if (roundedCent && largestPayer != null)
            {
                // person who paid the most on the trip pays the extra cent... OUCH!
                // negative in this case because someone had to over pay to 
                // enter this case, making their settlement value positive
                settlement[largestPayer] = greatestPaid - 0.01m;
            }

            return settlement;
        }

        /// <summary>
        /// Determine the appropiate way to distribute expenses evenly among
        /// all parties involved. During a tie precedence is given randomly.
        /// </summary>
        /// <param name="settlement">Transactions to be settled</param>
        /// <returns>
        /// List of human readible strings which describe how debts are to be
        /// settled.
        /// </returns>
        public static IEnumerable<Split> DetermineSplits(IDictionary<string, decimal> settlement)
        {
            var creditors = new List<string>();
            var debitors = new List<string>();
            var splits = new List<Split>();
            foreach (var transaction in settlement)
            {   // A person owing zero is removed from the calculation by design
                if (transaction.Value < 0)
                {
                    debitors.Add(transaction.Key);
                }
                else if (transaction.Value > 0)
                {
                    creditors.Add(transaction.Key);
                }
            }

            int totalCreditors = creditors.Count;
            int totalDebitors = debitors.Count;
            int j = 0;
            for (int i = 0; i < totalCreditors; i++)
            {
                string creditor = creditors[i];
                decimal currentCredit = settlement[creditor];
                for (; j < totalDebitors; j++)
                {
                    string debitor = debitors[j];
                    decimal currentDebt = Math.Abs(settlement[debitor]);
                    decimal delta = currentCredit - currentDebt;
                    if (delta < 0)
                    {
                        // debt greater than current credit, so we need to split
                        // the payment amongst two creditors
                        currentDebt -= currentCredit;
                        // update debt
                        settlement[debitor] = currentDebt;
                        splits.Add(new Split(creditor, debitor, currentCredit));
                        break;
                    }
                    else
                    {
                        currentCredit -= currentDebt;
                        splits.Add(new Split(creditor, debitor, currentDebt));
                    }
                }
            }

            return splits;
        }
    }
}