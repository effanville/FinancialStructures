using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml.Serialization;
using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures
{
    /// <summary>
    /// A container for the history of expenses.
    /// </summary>
    public interface IExpenseHistory : IXmlSerializable
    {
        /// <summary>
        /// The predicted expenses per month.
        /// </summary>
        IExpenseCollection PredictedExpenses
        {
            get;
        }

        /// <summary>
        /// Actual expenses by month. The Date is stored as a Year and Month, with day =1
        /// </summary>
        Dictionary<DateTime, IMonthlyExpenses> ActualExpensesByMonth
        {
            get;
        }

        /// <summary>
        /// Add a prediced expense.
        /// </summary>
        /// <param name="date">The date in the month for this expense.</param>
        /// <param name="amount">The amount of money.</param>
        /// <param name="category">The category of the expense.</param>
        /// <param name="notes">Any ancillary information.</param>
        void AddPredictedExpense(DateTime date, double amount, ExpenseCategory category, string notes = null);

        /// <summary>
        /// Add an actual expense.
        /// </summary>
        /// <param name="date">The date in the month for this expense.</param>
        /// <param name="amount">The amount of money.</param>
        /// <param name="category">The category of the expense.</param>
        /// <param name="notes">Any ancillary information.</param>
        void AddExpense(DateTime date, double amount, ExpenseCategory category, string notes = null);

        /// <summary>
        /// Write out the predicted expenses to file.
        /// </summary>
        void WritePredictedExpenses(string filepath, out string error);

        /// <summary>
        /// Write out the predicted expenses to file.
        /// </summary>
        void WritePredictedExpenses(IFileSystem fileSystem, string filepath, out string error);

        /// <summary>
        /// Writes the expenses in a specific month to a html file.
        /// </summary>
        /// <param name="filepath">The path to write the expenses.</param>
        /// <param name="month">The month expenses to export.</param>
        /// <param name="error">Any error from loading.</param>
        void WriteExpenses(string filepath, DateTime month, out string error);

        /// <summary>
        /// Writes the expenses in a specific month to a html file.
        /// </summary>
        /// <param name="fileSystem">The fileSystem to find the file in.</param>
        /// <param name="filepath">The path to write the expenses.</param>
        /// <param name="month">The month expenses to export.</param>
        /// <param name="error">Any error from loading.</param>
        void WriteExpenses(IFileSystem fileSystem, string filepath, DateTime month, out string error);
    }
}
