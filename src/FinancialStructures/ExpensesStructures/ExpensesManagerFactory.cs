using System.IO.Abstractions;

using FinancialStructures.ExpensesStructures.Implementation;

namespace FinancialStructures.ExpensesStructures
{
    /// <summary>
    /// Constains factory methods for <see cref="IExpensesManager"/>s.
    /// </summary>
    public static class ExpensesManagerFactory
    {
        /// <summary>
        /// Creates a new empty Expenses manager.
        /// </summary>
        public static IExpensesManager Create()
        {
            return new ExpensesManager();
        }

        /// <summary>
        /// Creates an Expense manager and loads from file.
        /// </summary>
        /// <param name="fileSystem">The fileSystem to find the file in.</param>
        /// <param name="filePath">The path to find the expenses.</param>
        /// <param name="error">Any error from loading.</param>
        public static IExpensesManager CreateFromFile(IFileSystem fileSystem, string filePath, out string error)
        {
            ExpensesManager em = new ExpensesManager();
            em.LoadFromFile(fileSystem, filePath, out error);
            return em;
        }
    }
}
