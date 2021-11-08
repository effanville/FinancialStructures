using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Xml.Serialization;

using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures
{
    /// <summary>
    /// A collection of expenses that allows for alteration of them.
    /// </summary>
    public interface IExpenseCollection : IXmlSerializable
    {
        /// <summary>
        /// The expenses in this collection.
        /// </summary>
        IReadOnlyList<Expense> Expenses
        {
            get;
        }

        /// <summary>
        /// Adds an expense
        /// </summary>
        void Add(Expense expense);

        /// <summary>
        /// Add an expense.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="category"></param>
        /// <param name="notes"></param>
        void Add(DateTime date, double amount, ExpenseCategory category, string notes = null);

        /// <summary>
        /// Remove an expense
        /// </summary>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <param name="category"></param>
        /// <param name="notes"></param>
        /// <returns>The numnber of expenses removed.</returns>
        int RemoveExpense(DateTime date, double amount, ExpenseCategory category, string notes = null);

        /// <summary>
        /// Remove an expense
        /// </summary>
        /// <param name="date"></param>
        /// <param name="amount"></param>
        /// <returns>The numnber of expenses removed.</returns>
        int RemoveExpense(DateTime date, double amount);

        /// <summary>
        /// The overall total value of expenses in the collection.
        /// </summary>
        double Total();

        /// <summary>
        /// Total value for the expenses in the given category.
        /// </summary>
        /// <param name="category">A <see cref="ExpenseCategory"/></param>
        double Total(ExpenseCategory category);

        /// <summary>
        /// Returns a list of all outgoing expenses.
        /// </summary>
        IReadOnlyList<Expense> Outgoings();

        /// <summary>
        /// Returns a list of all incoming expenses.
        /// </summary>
        IReadOnlyList<Expense> Incomings();

        /// <summary>
        /// Returns the total outgoing value of the expenses.
        /// </summary>
        double TotalOutgoings();

        /// <summary>
        /// Returns the total amount of expenses that are incoming.
        /// </summary>
        double TotalIncomings();

        /// <summary>
        /// Returns the number of expenses held.
        /// </summary>
        int Count();

        /// <summary>
        /// Writes the expenses in a specific month to a html file.
        /// </summary>
        /// <param name="fileSystem">The fileSystem to find the file in.</param>
        /// <param name="filepath">The path to write the expenses.</param>
        /// <param name="title">The title for the expenses</param>
        /// <param name="error">Any error from loading.</param>
        void WriteExpenses(IFileSystem fileSystem, string filepath, string title, out string error);

        /// <summary>
        /// Writes the expenses in a specific month to a html file.
        /// </summary>
        /// <param name="filepath">The path to write the expenses.</param>
        /// <param name="title">The title for the expenses</param>
        /// <param name="error">Any error from loading.</param>
        void WriteExpenses(string filepath, string title, out string error);
    }
}
