using System;
using System.IO;
using System.IO.Abstractions;
using System.Xml;
using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures.Implementation
{
    internal class ExpensesManager : IExpensesManager
    {
        /// <inheritdoc />
        public IExpenseHistory History
        {
            get;
        } = new ExpenseHistory();

        public ExpensesManager()
        {
        }

        /// <inheritdoc />
        public void AddPredictedExpense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            History.AddPredictedExpense(date, amount, category, notes);
        }

        /// <inheritdoc />
        public void AddExpense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            History.AddExpense(date, amount, category, notes);
        }
        /// <inheritdoc />
        public void LoadFromFile(string filepath, out string error)
        {
            LoadFromFile(new FileSystem(), filepath, out error);
        }

        /// <inheritdoc />
        public void LoadFromFile(IFileSystem fileSystem, string filepath, out string error)
        {
            error = null;
            try
            {
                XmlReaderSettings xmlReaderSettings = new XmlReaderSettings
                {
                    IgnoreWhitespace = true
                };

                using (Stream stream = fileSystem.FileStream.Create(filepath, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(stream, xmlReaderSettings))
                {
                    History.ReadXml(reader);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message + "InnerException:" + ex.InnerException?.Message;
            }
        }
        /// <inheritdoc />
        public void WriteToFile(string filepath, out string error)
        {
            WriteToFile(new FileSystem(), filepath, out error);
        }

        /// <inheritdoc />
        public void WriteToFile(IFileSystem fileSystem, string filepath, out string error)
        {
            error = null;
            try
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                {
                    NewLineOnAttributes = true,
                    Indent = true
                };

                using (Stream stream = fileSystem.FileStream.Create(filepath, FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    History.WriteXml(writer);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }

        /// <inheritdoc />
        public void WritePredictedExpenses(string filepath, out string error)
        {
            WritePredictedExpenses(new FileSystem(), filepath, out error);
        }

        /// <inheritdoc />
        public void WritePredictedExpenses(IFileSystem fileSystem, string filepath, out string error)
        {
            History.WritePredictedExpenses(fileSystem, filepath, out error);
        }
        /// <inheritdoc />
        public void WriteExpenses(string filepath, DateTime month, out string error)
        {
            WriteExpenses(new FileSystem(), filepath, month, out error);
        }

        /// <inheritdoc />
        public void WriteExpenses(IFileSystem fileSystem, string filepath, DateTime month, out string error)
        {
            History.WriteExpenses(fileSystem, filepath, month, out error);
        }

        /// <inheritdoc />
        public int PredictedCount()
        {
            return History.PredictedExpenses.Count();
        }

        /// <inheritdoc />
        public int MonthCount()
        {
            return History.ActualExpensesByMonth.Count;
        }

        /// <inheritdoc />
        public int Expenses(DateTime month)
        {
            return History.ActualExpensesByMonth.TryGetValue(month, out IMonthlyExpenses expenses) ? expenses.Count() : 0;
        }
    }
}
