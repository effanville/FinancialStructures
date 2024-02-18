using System;
using System.IO.Abstractions;
using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures
{
    /// <summary>
    /// Interface for the interaction with expenses.
    /// </summary>
    public interface IExpensesManager
    {
        /// <summary>
        /// Returns the history of the
        /// </summary>
        IExpenseHistory History
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
        /// Loads an <see cref="IExpensesManager"/> from file.
        /// </summary>
        /// <param name="filepath">The path to find the expenses.</param>
        /// <param name="error">Any error from loading.</param>
        void LoadFromFile(string filepath, out string error);

        /// <summary>
        /// Loads an <see cref="IExpensesManager"/> from file.
        /// </summary>
        /// <param name="fileSystem">The fileSystem to find the file in.</param>
        /// <param name="filepath">The path to find the expenses.</param>
        /// <param name="error">Any error from loading.</param>
        void LoadFromFile(IFileSystem fileSystem, string filepath, out string error);

        /// <summary>
        /// Saves an <see cref="IExpensesManager"/> to file.
        /// </summary>
        /// <param name="filepath">The path to find the expenses.</param>
        /// <param name="error">Any error from loading.</param>
        void WriteToFile(string filepath, out string error);

        /// <summary>
        /// Saves an <see cref="IExpensesManager"/> to file.
        /// </summary>
        /// <param name="fileSystem">The fileSystem to find the file in.</param>
        /// <param name="filepath">The path to find the expenses.</param>
        /// <param name="error">Any error from loading.</param>
        void WriteToFile(IFileSystem fileSystem, string filepath, out string error);

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

        /// <summary>
        /// The number of predicted expenses.
        /// </summary>
        int PredictedCount();

        /// <summary>
        /// The number of months of expenses.
        /// </summary>
        int MonthCount();

        /// <summary>
        /// The number of expenses in the given month.
        /// </summary>
        /// <param name="month">The month to query for.</param>
        /// <returns></returns>
        int Expenses(DateTime month);
    }
}
