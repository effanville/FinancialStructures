using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Common.Structure.FileAccess;

using FinancialStructures.ExpensesStructures.Models;

namespace FinancialStructures.ExpensesStructures.Implementation
{
    /// <summary>
    /// Contains a collection of expenses.
    /// </summary>
    internal class ExpenseCollection : IExpenseCollection, IEnumerable<Expense>, IXmlSerializable
    {
        private readonly object ExpenseLock = new object();

        private readonly List<Expense> ExpensesInternal = new List<Expense>();

        /// <inheritdoc/>
        public IReadOnlyList<Expense> Expenses
        {
            get
            {
                lock (ExpenseLock)
                {
                    return ExpensesInternal.ToList();
                }
            }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ExpenseCollection()
        {
        }

        /// <summary>
        /// Construct an instance.
        /// </summary>
        public ExpenseCollection(List<Expense> expenses)
        {
            ExpensesInternal = expenses;
        }

        /// <inheritdoc/>
        public void Add(Expense expense)
        {
            lock (ExpenseLock)
            {
                ExpensesInternal.Add(expense);
            }
        }

        /// <inheritdoc/>
        public void Add(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            var expense = new Expense(date, amount, category, notes);
            lock (ExpenseLock)
            {
                ExpensesInternal.Add(expense);
            }
        }

        /// <inheritdoc/>
        public int RemoveExpense(DateTime date, double amount, ExpenseCategory category, string notes = null)
        {
            int numRemoved;
            lock (ExpenseLock)
            {
                numRemoved = ExpensesInternal.RemoveAll(expense => expense.Date == date && expense.Amount == amount && expense.Category == category && expense.Notes == notes);
            }

            return numRemoved;
        }

        /// <inheritdoc/>
        public int RemoveExpense(DateTime date, double amount)
        {
            int numRemoved;
            lock (ExpenseLock)
            {
                numRemoved = ExpensesInternal.RemoveAll(expense => expense.Date == date && expense.Amount == amount);
            }

            return numRemoved;
        }

        /// <inheritdoc/>
        public double Total()
        {
            double sum = 0;
            foreach (var expense in Expenses)
            {
                sum += expense.Amount;
            }

            return sum;
        }

        /// <inheritdoc/>
        public double Total(ExpenseCategory category)
        {
            double sum = 0;
            foreach (var expense in Expenses)
            {
                if (expense.Category == category)
                {
                    sum += expense.Amount;
                }
            }

            return sum;
        }

        /// <inheritdoc/>
        public IReadOnlyList<Expense> Outgoings()
        {
            return Expenses.Where(x => x.Amount < 0.0).ToList();
        }

        /// <inheritdoc/>
        public IReadOnlyList<Expense> Incomings()
        {
            return Expenses.Where(x => x.Amount > 0.0).ToList();
        }

        /// <inheritdoc/>
        public double TotalOutgoings()
        {
            return SumExpensesInRange(double.NegativeInfinity, 0.0);
        }

        /// <inheritdoc/>
        public double TotalIncomings()
        {
            return SumExpensesInRange(0.0, double.PositiveInfinity);
        }

        /// <inheritdoc/>
        private double SumExpensesInRange(double lower, double upper)
        {
            double sum = 0.0;
            foreach (var expense in Expenses)
            {
                if (expense.Amount > lower && expense.Amount < upper)
                {
                    sum += expense.Amount;
                }
            }

            return sum;
        }

        /// <inheritdoc/>
        public int Count()
        {
            return Expenses.Count;
        }

        /// <inheritdoc/>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <inheritdoc/>
        public virtual void ReadXml(XmlReader reader)
        {
            bool isEmpty = reader.IsEmptyElement;
            reader.ReadStartElement("Expenses");

            if (!isEmpty)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    var expense = new Expense();
                    expense.ReadXml(reader);
                    ExpensesInternal.Add(expense);
                }

                reader.ReadEndElement();
            }
        }

        /// <inheritdoc/>
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("Expenses");
            foreach (var expense in Expenses)
            {
                expense.WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        /// <inheritdoc/>
        public void WriteExpenses(string filepath, string title, out string error)
        {
            WriteExpenses(new FileSystem(), filepath, title, out error);
        }

        /// <inheritdoc/>
        public void WriteExpenses(IFileSystem fileSystem, string filepath, string title, out string error)
        {
            error = null;
            try
            {
                using (Stream stream = fileSystem.FileStream.Create(filepath, FileMode.Create))
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.CreateHTMLHeader(title, true);
                    sw.WriteTitle(ExportType.Html, title);

                    sw.WriteTable(ExportType.Html, Expenses, false);
                    sw.WriteParagraph(ExportType.Html, new string[] { $"Total Expenses {Total()}" });

                    sw.CreateHTMLFooter();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }

        /// <inheritdoc/>
        public IEnumerator<Expense> GetEnumerator()
        {
            return Expenses.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Expenses.GetEnumerator();
        }
    }
}
