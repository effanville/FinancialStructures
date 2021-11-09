using System;

namespace FinancialStructures.ExpensesStructures
{
    /// <summary>
    /// An <see cref="IExpenseCollection"/> pertaining to expenses in a particular month.
    /// </summary>
    public interface IMonthlyExpenses : IExpenseCollection
    {
        /// <summary>
        /// The month associated to these expenses.
        /// </summary>
        DateTime Month
        {
            get;
        }
    }
}
